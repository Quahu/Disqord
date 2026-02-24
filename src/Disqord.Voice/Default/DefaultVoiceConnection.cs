using System;
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

public class DefaultVoiceConnection : IVoiceConnection
{
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
    private SpeakingFlags? _lastSpeakingFlags;

    private readonly object _stateLock = new();
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
            if (_gateway != null && (_gateway.SessionId != sessionId || ChannelId == channelId))
                return;

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

                _lastSpeakingFlags = flags;
                success = true;
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex is VoiceConnectionException)
                    throw;

                await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        while (!success);
    }

    public ValueTask SendPacketAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default)
    {
        return Udp.SendAsync(opus, cancellationToken);
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

                        Logger.LogDebug("Created voice gateway: Session ID: {0}, Token: {1}", voiceState.SessionId, voiceServer.Token);
                        _gateway = _gatewayFactory.Create(GuildId, CurrentMemberId, voiceState.SessionId, voiceServer.Token, voiceServer.Endpoint, Dave.IsAvailable ? Dave.MaxSupportedVersion : 0, Logger);

                        Gateway.SuspendAfterSessionDescription();
                        var gatewayRunTask = Gateway.RunAsync(linkedCancellationToken);

                        var readyModel = await Gateway.WaitForReadyAsync(linkedCancellationToken).ConfigureAwait(false);

                        var encryption = _encryptionProvider.GetEncryption(readyModel.Modes);
                        if (encryption == null || !readyModel.Modes.AsSpan().Contains(encryption.ModeName))
                        {
                            var exception = new VoiceConnectionException($"The encryption provider does not support any of the encryption modes that Discord returned ({string.Join(", ", readyModel.Modes)}).");
                            _readyTcs.Throw(exception);
                            return;
                        }

                        _udp = _udpFactory.Create(readyModel.Ssrc, readyModel.Ip, readyModel.Port, Logger, encryption);
                        await Udp.ConnectAsync(linkedCancellationToken).ConfigureAwait(false);

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
                        }, linkedCancellationToken).ConfigureAwait(false);

                        var sessionDescriptionModel = await Gateway.WaitForSessionDescriptionAsync(linkedCancellationToken).ConfigureAwait(false);
                        if (sessionDescriptionModel.DaveProtocolVersion is > 0)
                        {
                            if (!Dave.IsAvailable)
                            {
                                var exception = new VoiceConnectionException(
                                    $"The voice server requires DAVE end-to-end encryption (protocol version {sessionDescriptionModel.DaveProtocolVersion}), "
                                    + "but the native 'libdave' library could not be found. "
                                    + "Ensure the native library is available in the application's search path.");

                                _readyTcs.Throw(exception);
                                return;
                            }

                            _daveHandler = new DaveProtocolHandler(Gateway, (ushort) sessionDescriptionModel.DaveProtocolVersion, GuildId, CurrentMemberId, _loggerFactory);
                            await _daveHandler.InitializeAsync(linkedCancellationToken).ConfigureAwait(false);
                        }

                        Gateway.ResumeAfterSessionDescription(_daveHandler != null ? _daveHandler.HandleMessageAsync : null);

                        Udp.Initialize(sessionDescriptionModel.SecretKey, _daveHandler?.Encryptor);

                        _synchronizer.Subscribe(_udp);

                        if (_lastSpeakingFlags != null)
                        {
                            await SetSpeakingFlagsAsync(_lastSpeakingFlags.Value, linkedCancellationToken).ConfigureAwait(false);
                        }

                        lock (_stateLock)
                        {
                            _readyTcs.Complete();
                        }

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
                        lastCloseCode = VoiceGatewayCloseCode.ForciblyDisconnected;
                    }
                    catch (WebSocketClosedException ex)
                    {
                        lastCloseCode = (VoiceGatewayCloseCode?) ex.CloseStatus;
                    }
                    finally
                    {
                        var gatewayDisposeTask = default(ValueTask);
                        var udpCloseTask = default(ValueTask);
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

                            _daveHandler?.Dispose();
                            _daveHandler = null;

                            if (_readyTcs.Task.IsCompleted)
                            {
                                _readyTcs = new Tcs();
                            }
                        }

                        await gatewayDisposeTask.ConfigureAwait(false);
                        await udpCloseTask.ConfigureAwait(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await _setVoiceStateDelegate(GuildId, null, default).ConfigureAwait(false);

            lock (_readyTcs)
            {
                _readyTcs.Throw(ex);
            }

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
        _stateUpdateCts.Dispose();

        if (_udp != null)
        {
            _synchronizer.Unsubscribe(_udp);
            _udp.Dispose();
        }

        _daveHandler?.Dispose();

        if (_gateway != null)
        {
            return Gateway.DisposeAsync();
        }

        return default;
    }
}
