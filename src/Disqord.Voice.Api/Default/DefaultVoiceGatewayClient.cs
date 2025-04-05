using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Disqord.Utilities.Threading;
using Disqord.Voice.Api.Models;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGatewayClient : IVoiceGatewayClient
{
    public Snowflake GuildId { get; }

    public Snowflake CurrentMemberId { get; }

    public string SessionId { get; }

    public string Token { get; }

    public string Endpoint { get; }

    public ILogger Logger { get; }

    public IVoiceGatewayHeartbeater Heartbeater { get; }

    public IVoiceGateway Gateway { get; }

    public IJsonSerializer Serializer { get; }

    public CancellationToken StoppingToken { get; private set; }

    private readonly object _stateLock = new();
    private Tcs<ReadyJsonModel> _readyTcs;
    private Tcs<SessionDescriptionJsonModel> _sessionDescriptionTcs;

    public DefaultVoiceGatewayClient(
        Snowflake guildId,
        Snowflake currentMemberId,
        string sessionId,
        string token,
        string endpoint,
        ILogger logger,
        IVoiceGatewayHeartbeater heartbeater,
        IVoiceGateway gateway,
        IJsonSerializer serializer)
    {
        GuildId = guildId;
        CurrentMemberId = currentMemberId;
        Logger = logger;
        Heartbeater = heartbeater;
        Heartbeater.Bind(this);
        Gateway = gateway;
        Gateway.Bind(this);
        Serializer = serializer;

        SessionId = sessionId;
        Token = token;
        Endpoint = endpoint;

        _readyTcs = new Tcs<ReadyJsonModel>();
        _sessionDescriptionTcs = new Tcs<SessionDescriptionJsonModel>();
    }

    /// <inheritdoc/>
    public Task<ReadyJsonModel> WaitForReadyAsync(CancellationToken cancellationToken)
    {
        Task<ReadyJsonModel> readyTask;
        lock (_stateLock)
        {
            readyTask = _readyTcs.Task;
        }

        return readyTask.WaitAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<SessionDescriptionJsonModel> WaitForSessionDescriptionAsync(CancellationToken cancellationToken)
    {
        Task<SessionDescriptionJsonModel> sessionDescriptionTask;
        lock (_stateLock)
        {
            sessionDescriptionTask = _sessionDescriptionTcs.Task;
        }

        return sessionDescriptionTask.WaitAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SendAsync(VoiceGatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(payload);

        var sent = false;
        do
        {
            try
            {
                Logger.LogTrace("Sending voice payload: {0}.", payload.Op);
                await Gateway.SendAsync(payload, cancellationToken).ConfigureAwait(false);
                sent = true;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    Logger.LogError(ex, "An exception occurred while sending voice payload: {0}.", payload.Op);

                throw;
            }
        }
        while (!sent);
    }

    public Task RunAsync(CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        return InternalRunAsync(stoppingToken);
    }

    private async Task InternalConnectAsync(CancellationToken stoppingToken)
    {
        var attempt = 0;
        var delay = 2500;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (attempt == 0)
                {
                    Logger.LogDebug("Connecting to the voice gateway...");
                }
                else
                {
                    Logger.LogDebug("Retrying (attempt {Attempt}) connecting to the voice gateway...", attempt + 1);
                }

                await Gateway.ConnectAsync(new Uri(Endpoint), stoppingToken).ConfigureAwait(false);
                return;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while connecting to the voice gateway.");
                if (attempt++ < 6)
                    delay *= 2;

                Logger.LogInformation("Delaying the retry for {0}ms.", delay);
                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
            }
        }

        stoppingToken.ThrowIfCancellationRequested();
        Logger.LogInformation("Successfully connected.");
    }

    private async Task InternalRunAsync(CancellationToken stoppingToken)
    {
        var resume = false;
        while (!stoppingToken.IsCancellationRequested)
        {
            await InternalConnectAsync(stoppingToken).ConfigureAwait(false);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var payload = await Gateway.ReceiveAsync(stoppingToken).ConfigureAwait(false);
                    Logger.LogTrace("Received voice payload: {0}.", payload.Op);

                    switch (payload.Op)
                    {
                        case VoiceGatewayPayloadOperation.Ready:
                        {
                            Logger.LogDebug("Successfully identified. The voice gateway is ready.");
                            var model = payload.D!.ToType<ReadyJsonModel>()!;

                            Logger.LogTrace("SSRC: {Ssrc}, IP: {Ip}:{Port}, Modes: {Modes}.", model.Ssrc, model.Ip, model.Port, model.Modes);

                            lock (_stateLock)
                            {
                                _readyTcs.Complete(model);
                            }

                            break;
                        }
                        case VoiceGatewayPayloadOperation.Heartbeat:
                        {
                            Logger.LogDebug("The voice gateway requested a heartbeat.");
                            await Heartbeater.HeartbeatAsync(stoppingToken).ConfigureAwait(false);
                            break;
                        }
                        case VoiceGatewayPayloadOperation.SessionDescription:
                        {
                            var model = payload.D!.ToType<SessionDescriptionJsonModel>()!;
                            Logger.LogDebug("The voice gateway sent a session description with mode {0}.", model.Mode);
                            _sessionDescriptionTcs.Complete(model);
                            break;
                        }
                        case VoiceGatewayPayloadOperation.Speaking:
                        {
                            // TODO: speaking, but it's bugged, after initial connect it fires once per user, after a reconnect it fires constantly
                            break;
                        }
                        case VoiceGatewayPayloadOperation.HeartbeatAcknowledged:
                        {
                            try
                            {
                                await Heartbeater.AcknowledgeAsync().ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "An exception occurred while acknowledging the heartbeat.");
                            }

                            break;
                        }
                        case VoiceGatewayPayloadOperation.Hello:
                        {
                            Logger.LogDebug("The voice gateway said hello.");
                            try
                            {
                                var model = payload.D!.ToType<HelloJsonModel>()!;
                                var interval = TimeSpan.FromMilliseconds(model.HeartbeatInterval);
                                await Heartbeater.StartAsync(interval).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "An exception occurred while starting the voice heartbeater.");
                            }

                            if (!resume)
                            {
                                Logger.LogDebug("Identifying voice...");
                                resume = true;
                                await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                            }
                            else
                            {
                                Logger.LogDebug("Resuming voice...");
                                await ResumeAsync(stoppingToken).ConfigureAwait(false);
                            }

                            break;
                        }
                        case VoiceGatewayPayloadOperation.Resumed:
                        {
                            Logger.LogDebug("The voice gateway resumed the session.");
                            break;
                        }
                        case VoiceGatewayPayloadOperation.ClientConnect:
                        {
                            // TODO: client connect
                            break;
                        }
                        case VoiceGatewayPayloadOperation.ClientDisconnect:
                        {
                            // TODO: client disconnect
                            break;
                        }
                        default:
                        {
                            Logger.LogWarning("Unknown voice gateway operation: {0}.", payload.Op);
                            break;
                        }
                    }
                }
            }
            catch (WebSocketClosedException ex)
            {
                if (ex.CloseStatus != null)
                {
                    var closeCode = (VoiceGatewayCloseCode) ex.CloseStatus.Value;
                    if (closeCode.IsRecoverable())
                    {
                        Logger.LogInformation("The voice gateway was closed with code {0} and reason '{1}'.", closeCode, ex.CloseMessage);
                    }
                    else
                    {
                        var level = closeCode == VoiceGatewayCloseCode.ForciblyDisconnected
                            ? LogLevel.Information
                            : LogLevel.Warning;

                        Logger.Log(level, "The voice gateway was closed with code {0} indicating a non-recoverable connection - stopping.", closeCode);
                        _readyTcs.Throw(ex);
                        _sessionDescriptionTcs.Throw(ex);
                        throw;
                    }
                }
                else
                {
                    Logger.LogWarning(ex, "The voice gateway was closed with an exception.");
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogInformation("The voice gateway run was cancelled.");

                try
                {
                    await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exc, "An exception occurred while closing the voice gateway connection after cancellation.");
                }

                _readyTcs.Throw(ex);
                _sessionDescriptionTcs.Throw(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "An exception occurred while receiving a voice payload. Stopping the connection.");
                try
                {
                    await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exc, "An exception occurred while closing the voice gateway connection after cancellation.");
                }

                _readyTcs.Throw(ex);
                _sessionDescriptionTcs.Throw(ex);
                throw;
            }
            finally
            {
                try
                {
                    Logger.LogDebug("Stopping the voice heartbeater due to connection end.");
                    await Heartbeater.StopAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An exception occurred while stopping the voice heartbeater.");
                }
            }
        }
    }

    private Task IdentifyAsync(CancellationToken cancellationToken)
    {
        return SendAsync(new VoiceGatewayPayloadJsonModel
        {
            Op = VoiceGatewayPayloadOperation.Identify,
            D = new IdentifyJsonModel
            {
                ServerId = GuildId,
                UserId = CurrentMemberId,
                SessionId = SessionId,
                Token = Token
            }
        }, cancellationToken);
    }

    private Task ResumeAsync(CancellationToken cancellationToken)
    {
        return SendAsync(new VoiceGatewayPayloadJsonModel
        {
            Op = VoiceGatewayPayloadOperation.Resume,
            D = new ResumeJsonModel
            {
                ServerId = GuildId,
                SessionId = SessionId,
                Token = Token
            }
        }, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return Gateway.DisposeAsync();
    }
}
