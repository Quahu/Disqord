using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Disqord.Voice.Api;
using Disqord.Voice.Api.Models;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Voice.Default;

public class DefaultVoiceConnection : IVoiceConnectionHost
{
    private volatile VoicePacketSinkDelegate? _packetSink;

    public ILogger Logger { get; }

    public IVoiceGatewayClient Gateway => _gateway!;

    public IVoiceUdpClient Udp => _udp!;

    public IVoiceSynchronizer Synchronizer => _synchronizer;

    public Snowflake GuildId { get; }

    public Snowflake ChannelId
    {
        get
        {
            lock (_stateLock)
            {
                return _channelId;
            }
        }
    }

    public Snowflake CurrentMemberId { get; }

    private static readonly IReadOnlySet<Snowflake> EmptyUserIds = new HashSet<Snowflake>();

    public IReadOnlySet<Snowflake> ConnectedUserIds => _gateway?.ConnectedUserIds ?? EmptyUserIds;

    public event VoiceUserPresenceDelegate? UserConnected;

    public event VoiceUserPresenceDelegate? UserDisconnected;

    private readonly SetVoiceStateDelegate _setVoiceStateDelegate;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IVoiceGatewayClientFactory _gatewayFactory;
    private readonly IVoiceUdpClientFactory _udpFactory;
    private readonly IVoiceSynchronizer _synchronizer;
    private readonly IVoiceEncryptionProvider _encryptionProvider;

    private IVoiceGatewayClient? _gateway;
    private IVoiceUdpClient? _udp;
    private DaveProtocolHandler? _daveHandler;
    private Snowflake _channelId;
    private volatile int _lastSpeakingFlags = -1;
    private Cts? _receiveCts;
    private Task? _receiveTask;

    private readonly object _stateLock = new();
    private readonly object _receiveLock = new();
    private volatile bool _disposed;
    private Cts _stateUpdateCts;

    private Tcs _readyTcs;
    private Tcs<(Snowflake? ChannelId, string SessionId)> _voiceStateUpdateTcs;
    private Tcs<(string Token, string? Endpoint)> _voiceServerUpdateTcs;

    public DefaultVoiceConnection(
        Snowflake guildId,
        Snowflake channelId,
        Snowflake currentMemberId,
        SetVoiceStateDelegate setVoiceStateDelegate,
        ILoggerFactory loggerFactory,
        IVoiceGatewayClientFactory gatewayFactory,
        IVoiceUdpClientFactory udpFactory,
        IVoiceSynchronizer synchronizer,
        IVoiceEncryptionProvider encryptionProvider)
    {
        Logger = loggerFactory.CreateLogger($"Voice {guildId}");
        _loggerFactory = loggerFactory;
        _gatewayFactory = gatewayFactory;
        _udpFactory = udpFactory;
        _synchronizer = synchronizer;
        _encryptionProvider = encryptionProvider;

        GuildId = guildId;
        _channelId = channelId;
        CurrentMemberId = currentMemberId;
        _setVoiceStateDelegate = setVoiceStateDelegate;

        _stateUpdateCts = new();
        _readyTcs = new();
        _voiceStateUpdateTcs = new();
        _voiceServerUpdateTcs = new();
    }

    [Conditional("DEBUG")]
    private void OnStateUpdate()
    {
        Guard.IsTrue(Monitor.IsEntered(_stateLock));
    }

    public void OnVoiceStateUpdate(Snowflake? channelId, string sessionId)
    {
        Logger.LogTrace("VOICE_STATE_UPDATE with Channel ID: {0}, Session ID: {1}", channelId, sessionId);

        lock (_stateLock)
        {
            if (_gateway != null && (_gateway.SessionId != sessionId || _channelId == channelId))
            {
                return;
            }

            OnStateUpdate();

            if (channelId != null)
            {
                _channelId = channelId.Value;
            }
            else
            {
                if (_gateway != null
                    && !_stateUpdateCts.IsCancellationRequested)
                {
                    // Notifies RunAsync we've been disconnected.
                    _stateUpdateCts.Cancel();
                }
            }

            var voiceState = (channelId, sessionId);
            if (!_voiceStateUpdateTcs.Complete(voiceState))
            {
                _voiceStateUpdateTcs = new();
                _voiceStateUpdateTcs.Complete(voiceState);
            }
        }
    }

    public void OnVoiceServerUpdate(string token, string? endpoint)
    {
        Logger.LogTrace("VOICE_SERVER_UPDATE with Token: {0}, Endpoint: {1}", token, endpoint);

        lock (_stateLock)
        {
            OnStateUpdate();

            var voiceServer = (token, endpoint);
            if (!_voiceServerUpdateTcs.Complete(voiceServer))
            {
                _voiceServerUpdateTcs = new();
                _voiceServerUpdateTcs.Complete(voiceServer);
            }

            if (_gateway != null
                && !_stateUpdateCts.IsCancellationRequested)
            {
                // Notifies RunAsync the connection data has changed.
                _stateUpdateCts.Cancel();
            }
        }
    }

    public async ValueTask SetChannelIdAsync(Snowflake channelId, CancellationToken cancellationToken = default)
    {
        await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
        await _setVoiceStateDelegate(GuildId, channelId, cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask SetSpeakingFlagsAsync(SpeakingFlags flags, CancellationToken cancellationToken = default)
    {
        var success = false;
        do
        {
            try
            {
                await Gateway.SendAsync(new VoiceGatewayPayloadJsonModel
                {
                    Op = VoiceGatewayPayloadOperation.Speaking,
                    D = new SpeakingJsonModel
                    {
                        Delay = 0,
                        Speaking = flags,
                        Ssrc = Udp.Ssrc
                    }
                }, cancellationToken).ConfigureAwait(false);

                _lastSpeakingFlags = (int) flags;
                success = true;
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                // The connection is shutting down; sending Speaking is best-effort.
                return;
            }
            catch (Exception ex)
            {
                if (ex is VoiceConnectionException)
                {
                    throw;
                }

                await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        while (!success);
    }

    public ValueTask SendPacketAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default)
    {
        var udp = _udp;
        if (udp != null)
        {
            return udp.SendAsync(opus, cancellationToken);
        }

        return SendPacketAfterReadyAsync(opus, cancellationToken);
    }

    private async ValueTask SendPacketAfterReadyAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken)
    {
        await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
        await Udp.SendAsync(opus, cancellationToken).ConfigureAwait(false);
    }

    public ValueTask SetPacketSinkAsync(VoicePacketSinkDelegate? sink, CancellationToken cancellationToken = default)
    {
        _packetSink = sink;

        lock (_receiveLock)
        {
            if (sink != null)
            {
                TryStartReceiveLoop(CancellationToken.None);
            }
            else
            {
                StopReceiveLoop();
            }
        }

        return default;
    }

    private void TryStartReceiveLoop(CancellationToken parentToken = default)
    {
        Debug.Assert(Monitor.IsEntered(_receiveLock));

        if (_packetSink == null)
        {
            return;
        }

        if (_udp == null)
        {
            return;
        }

        if (_receiveTask != null && !_receiveTask.IsCompleted)
        {
            return;
        }

        _receiveCts?.Dispose();
        _receiveCts = parentToken.CanBeCanceled ? Cts.Linked(parentToken) : new Cts();
        _receiveTask = RunReceiveLoopAsync(_receiveCts.Token);
    }

    private void StopReceiveLoop()
    {
        Debug.Assert(Monitor.IsEntered(_receiveLock));

        _receiveCts?.Cancel();
    }

    private async Task RunReceiveLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            VoiceReceivePacket? packet;
            try
            {
                packet = await Udp.ReceiveAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested || _disposed)
            {
                return;
            }
            catch (Exception ex)
            {
                if (_disposed)
                {
                    return;
                }

                Logger.LogError(ex, "An exception occurred while receiving a voice packet.");
                continue;
            }

            if (packet == null)
            {
                continue;
            }

            var mappedPacket = packet.Value;
            if (mappedPacket.UserId == null)
            {
                var gateway = _gateway;
                if (gateway != null && gateway.TryGetUserId(mappedPacket.Ssrc, out var userId))
                {
                    mappedPacket.UserId = userId;
                }
            }

            var sink = _packetSink;
            if (sink != null)
            {
                try
                {
                    var task = sink(mappedPacket);
                    if (!task.IsCompletedSuccessfully)
                    {
                        await task.ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An exception occurred in the voice packet sink.");
                }
            }
            else
            {
                mappedPacket.Dispose();
            }
        }
    }

    /// <summary>
    ///     Tears down the current session's gateway, UDP, DAVE, and receive loop resources.
    ///     Called in the <c>finally</c> block of <see cref="RunAsync"/> after each connection attempt.
    /// </summary>
    private async Task CleanupSessionResourcesAsync()
    {
        var gatewayDisposeTask = default(ValueTask);
        var udpCloseTask = default(ValueTask);

        lock (_receiveLock)
        {
            StopReceiveLoop();
        }

        // Notify listeners that all users have disconnected before tearing down the gateway.
        if (_gateway != null)
        {
            foreach (var userId in _gateway.ConnectedUserIds)
                UserDisconnected?.Invoke(userId);
        }

        lock (_stateLock)
        {
            if (_gateway != null)
            {
                gatewayDisposeTask = _gateway.DisposeAsync();
                _gateway = null;
            }

            if (_udp != null)
            {
                udpCloseTask = _udp.CloseAsync(default);
                _synchronizer.Unsubscribe(_udp);
                _udp.Dispose();
                _udp = null;
            }

            if (_readyTcs.Task.IsCompleted)
            {
                _readyTcs = new Tcs();
            }
        }

        await gatewayDisposeTask.ConfigureAwait(false);
        await udpCloseTask.ConfigureAwait(false);

        // Await receive loop completion BEFORE disposing the DAVE handler.
        // The receive loop holds references to DaveDecryptor instances obtained
        // via GetDecryptor(); disposing the handler (which calls ClearDecryptors)
        // while a decrypt operation is in-flight would free native handles prematurely.
        if (_receiveTask != null)
        {
            try
            {
                await _receiveTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            { }

            _receiveTask = null;
        }

        _daveHandler?.Dispose();
        _daveHandler = null;

        _receiveCts?.Dispose();
        _receiveCts = null;
    }

    /// <summary>
    ///     Creates and configures the voice gateway, UDP client, and (optionally) DAVE handler
    ///     for a single session, then marks the connection as ready.
    /// </summary>
    /// <returns> The gateway run task to await for the lifetime of this session. </returns>
    private async Task<Task> EstablishSessionAsync(
        string sessionId, string token, string endpoint,
        CancellationToken cancellationToken)
    {
        Logger.LogDebug("Created voice gateway: Session ID: {0}, Token: {1}", sessionId, token);
        _gateway = _gatewayFactory.Create(GuildId, CurrentMemberId, sessionId, token, endpoint, Dave.IsAvailable ? Dave.MaxSupportedVersion : 0, Logger);

        Gateway.UserConnected = userId => UserConnected?.Invoke(userId);
        Gateway.UserDisconnected = userId => UserDisconnected?.Invoke(userId);

        Gateway.SuspendAfterSessionDescription();
        var gatewayRunTask = Gateway.RunAsync(cancellationToken);

        var readyModel = await Gateway.WaitForReadyAsync(cancellationToken).ConfigureAwait(false);

        var encryption = _encryptionProvider.GetEncryption(readyModel.Modes);
        if (encryption == null || !readyModel.Modes.AsSpan().Contains(encryption.ModeName))
        {
            throw new VoiceConnectionException($"The encryption provider does not support any of the encryption modes that Discord returned ({string.Join(", ", readyModel.Modes)}).");
        }

        _udp = _udpFactory.Create(readyModel.Ssrc, readyModel.Ip, readyModel.Port, Logger, encryption);
        await Udp.ConnectAsync(cancellationToken).ConfigureAwait(false);

        await Gateway.SendAsync(new VoiceGatewayPayloadJsonModel
        {
            Op = VoiceGatewayPayloadOperation.SelectProtocol,
            D = new SelectProtocolJsonModel
            {
                Protocol = "udp",
                Data = new SelectProtocolDataJsonModel
                {
                    Address = Udp.RemoteHostName!,
                    Port = Udp.RemotePort!.Value,
                    Mode = encryption.ModeName
                }
            }
        }, cancellationToken).ConfigureAwait(false);

        var sessionDescriptionModel = await Gateway.WaitForSessionDescriptionAsync(cancellationToken).ConfigureAwait(false);
        if (sessionDescriptionModel.DaveProtocolVersion is > 0)
        {
            if (!Dave.IsAvailable)
            {
                throw new VoiceConnectionException(
                    $"The voice server requires DAVE end-to-end encryption (protocol version {sessionDescriptionModel.DaveProtocolVersion}), "
                    + "but the native 'libdave' library could not be found. "
                    + "Ensure the native library is available in the application's search path.");
            }

            _daveHandler = new DaveProtocolHandler(Gateway, (ushort) sessionDescriptionModel.DaveProtocolVersion, GuildId, CurrentMemberId, _loggerFactory);
            await _daveHandler.InitializeAsync(cancellationToken).ConfigureAwait(false);
        }

        Gateway.ResumeAfterSessionDescription(_daveHandler != null ? _daveHandler.HandleMessageAsync : null);

        Udp.Initialize(sessionDescriptionModel.SecretKey, _daveHandler?.Encryptor);

        if (_udp is DefaultVoiceUdpClient defaultUdp)
        {
            defaultUdp.SetDaveHandler(_daveHandler, _daveHandler != null ? Gateway : null);
        }

        _synchronizer.Subscribe(_udp);

        lock (_receiveLock)
        {
            TryStartReceiveLoop(cancellationToken);
        }

        var lastSpeakingFlags = _lastSpeakingFlags;
        if (lastSpeakingFlags >= 0)
        {
            await SetSpeakingFlagsAsync((SpeakingFlags) lastSpeakingFlags, cancellationToken).ConfigureAwait(false);
        }

        lock (_stateLock)
        {
            _readyTcs.Complete();
        }

        return gatewayRunTask;
    }

    public async Task RunAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Yield();

            VoiceGatewayCloseCode? lastCloseCode = null;
            while (!stoppingToken.IsCancellationRequested)
            {
                Task<(Snowflake? ChannelId, string SessionId)> voiceStateUpdateTask;
                Task<(string Token, string? Endpoint)> voiceServerUpdateTask;
                CancellationToken stateCancellationToken;
                lock (_stateLock)
                {
                    if (_stateUpdateCts.IsCancellationRequested)
                    {
                        _stateUpdateCts.Dispose();
                        _stateUpdateCts = new();
                    }

                    stateCancellationToken = _stateUpdateCts.Token;

                    voiceStateUpdateTask = _voiceStateUpdateTcs.Task;
                    if (voiceStateUpdateTask.IsCompleted)
                    {
                        _voiceStateUpdateTcs = new();
                    }

                    voiceServerUpdateTask = _voiceServerUpdateTcs.Task;
                    if (voiceServerUpdateTask.IsCompleted)
                    {
                        _voiceServerUpdateTcs = new();
                    }
                }

                using (var linkedCts = Cts.Linked(stateCancellationToken, stoppingToken))
                {
                    var linkedCancellationToken = linkedCts.Token;
                    while (true)
                    {
                        if (lastCloseCode != VoiceGatewayCloseCode.ForciblyDisconnected)
                        {
                            await _setVoiceStateDelegate(GuildId, ChannelId, linkedCancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            if (!voiceStateUpdateTask.IsCompletedSuccessfully
                                || voiceStateUpdateTask.Result.ChannelId != null)
                            {
                                if (voiceServerUpdateTask.IsCompletedSuccessfully
                                    || await Task.WhenAny(voiceServerUpdateTask, Task.Delay(5_000, linkedCancellationToken)).ConfigureAwait(false) == voiceServerUpdateTask)
                                {
                                    lastCloseCode = null;
                                    continue;
                                }
                            }

                            linkedCancellationToken.ThrowIfCancellationRequested();
                            var exception = new VoiceConnectionException("Forcibly disconnected from the voice channel.");
                            _readyTcs.Throw(exception);
                            await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);
                            return;
                        }

                        var timeoutTask = Task.Delay(10_000, linkedCancellationToken);
                        var completedTask = await Task.WhenAny(
                                Task.WhenAll(voiceStateUpdateTask, voiceServerUpdateTask)
                                    .WaitAsync(linkedCancellationToken),
                                timeoutTask)
                            .ConfigureAwait(false);

                        linkedCancellationToken.ThrowIfCancellationRequested();
                        if (completedTask == timeoutTask)
                        {
                            var exception = new VoiceConnectionException("Failed to receive state updates in time.");
                            _readyTcs.Throw(exception);
                            await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);
                            return;
                        }

                        break;
                    }

                    try
                    {
                        var voiceServer = voiceServerUpdateTask.Result;
                        if (voiceServer.Endpoint == null)
                        {
                            Logger.LogInformation("The voice server is currently being reallocated - awaiting endpoint...");
                            continue;
                        }

                        var voiceState = voiceStateUpdateTask.Result;
                        var gatewayRunTask = await EstablishSessionAsync(voiceState.SessionId, voiceServer.Token, voiceServer.Endpoint, linkedCancellationToken).ConfigureAwait(false);
                        await gatewayRunTask.ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);
                        _readyTcs.Cancel(stoppingToken);
                        return;
                    }
                    catch (OperationCanceledException) when (stateCancellationToken.IsCancellationRequested)
                    {
                        if (_disposed)
                        {
                            await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);
                            return;
                        }

                        lastCloseCode = VoiceGatewayCloseCode.ForciblyDisconnected;
                    }
                    catch (WebSocketClosedException ex)
                    {
                        lastCloseCode = (VoiceGatewayCloseCode?) ex.CloseStatus;
                    }
                    finally
                    {
                        await CleanupSessionResourcesAsync().ConfigureAwait(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);

            _readyTcs.Throw(ex);

            Logger.LogError(ex, "An exception occurred in the voice connection.");
        }
    }

    /// <inheritdoc/>
    public Task WaitUntilReadyAsync(CancellationToken cancellationToken = default)
    {
        Task readyTask;
        lock (_stateLock)
        {
            readyTask = _readyTcs.Task;
        }

        return readyTask.WaitAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        lock (_receiveLock)
        {
            StopReceiveLoop();
        }

        if (_gateway != null)
        {
            foreach (var userId in _gateway.ConnectedUserIds)
                UserDisconnected?.Invoke(userId);
        }

        IVoiceGatewayClient? gateway;
        IVoiceUdpClient? udp;
        DaveProtocolHandler? daveHandler;
        lock (_stateLock)
        {
            _disposed = true;

            if (!_stateUpdateCts.IsCancellationRequested)
            {
                _stateUpdateCts.Cancel();
            }

            if (!_readyTcs.Task.IsCompleted)
            {
                _readyTcs.Throw(new VoiceConnectionException("The voice connection has been disposed."));
            }

            gateway = _gateway;
            _gateway = null;

            udp = _udp;
            _udp = null;

            daveHandler = _daveHandler;
            _daveHandler = null;
        }

        return DisposeAsyncCore(udp, daveHandler, gateway);
    }

    private async ValueTask DisposeAsyncCore(IVoiceUdpClient? udp, DaveProtocolHandler? daveHandler, IVoiceGatewayClient? gateway)
    {
        // Await receive loop before disposing resources it depends on.
        if (_receiveTask != null)
        {
            try
            {
                await _receiveTask.ConfigureAwait(false);
            }
            catch
            { }

            _receiveTask = null;
        }

        _receiveCts?.Dispose();
        _receiveCts = null;

        _stateUpdateCts.Dispose();

        if (udp != null)
        {
            _synchronizer.Unsubscribe(udp);
            udp.Dispose();
        }

        daveHandler?.Dispose();

        if (gateway != null)
        {
            await gateway.DisposeAsync().ConfigureAwait(false);
        }
    }
}
