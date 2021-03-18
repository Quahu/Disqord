using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Disqord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Api.Default
{
    public class DefaultGatewayApiClient : IGatewayApiClient
    {
        public Token Token { get; }

        public GatewayIntents Intents { get; }

        public int LargeThreshold { get; }

        public ShardId Id { get; }

        public UpdatePresenceJsonModel Presence { get; }

        public ILogger Logger { get; }

        public IGatewayRateLimiter RateLimiter { get; }

        public IGatewayHeartbeater Heartbeater { get; }

        public IGateway Gateway { get; }

        public IJsonSerializer Serializer { get; }

        public string SessionId { get; private set; }

        public int? Sequence { get; private set; }

        public CancellationToken StoppingToken { get; private set; }

        private readonly Random _random;

        public DefaultGatewayApiClient(
            IOptions<DefaultGatewayApiClientConfiguration> options,
            ILogger<DefaultGatewayApiClient> logger,
            Token token,
            IGatewayRateLimiter rateLimiter,
            IGatewayHeartbeater heartbeater,
            IGateway gateway,
            IJsonSerializer serializer)
            : this(options, logger as ILogger, token, rateLimiter, heartbeater, gateway, serializer)
        { }

        [ActivatorUtilitiesConstructor]
        public DefaultGatewayApiClient(
            IOptions<DefaultGatewayApiClientConfiguration> options,
            ILogger logger,
            Token token,
            IGatewayRateLimiter rateLimiter,
            IGatewayHeartbeater heartbeater,
            IGateway gateway,
            IJsonSerializer serializer)
        {
            var configuration = options.Value;
            Intents = configuration.Intents;
            LargeThreshold = configuration.LargeThreshold;
            Id = configuration.Id;
            Presence = configuration.Presence;
            Logger = logger;
            Token = token;
            RateLimiter = rateLimiter;
            RateLimiter.Bind(this);
            Heartbeater = heartbeater;
            Heartbeater.Bind(this);
            Gateway = gateway;
            Gateway.Bind(this);
            Serializer = serializer;

            _random = new Random();
        }

        public event AsynchronousEventHandler<GatewayDispatchReceivedEventArgs> DispatchReceived
        {
            add => _dispatchReceivedEvent.Hook(value);
            remove => _dispatchReceivedEvent.Unhook(value);
        }
        private readonly AsynchronousEvent<GatewayDispatchReceivedEventArgs> _dispatchReceivedEvent = new();

        public async Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            await RateLimiter.WaitAsync(payload.Op, cancellationToken).ConfigureAwait(false);
            try
            {
                Logger.LogTrace("Sending payload: {0}.", payload.Op);
                await Gateway.SendAsync(payload, cancellationToken).ConfigureAwait(false);
                RateLimiter.NotifyCompletion(payload.Op);
            }
            catch
            {
                RateLimiter.Release(payload.Op);
                throw;
            }
        }

        public Task RunAsync(Uri uri, CancellationToken stoppingToken)
        {
            StoppingToken = stoppingToken;
            return InternalRunAsync(uri, stoppingToken);
        }

        private async Task InternalConnectAsync(Uri uri, CancellationToken stoppingToken)
        {
            var attempt = 0;
            var delay = 2500;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Logger.LogDebug(attempt == 0
                        ? "Connecting to the gateway..."
                        : $"Retrying (attempt #{attempt + 1}) to connect to the gateway...");
                    await Gateway.ConnectAsync(uri, stoppingToken).ConfigureAwait(false);
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An exception occurred while connecting to the gateway.");
                    if (attempt++ < 6)
                        delay *= 2;

                    Logger.LogInformation("Delaying the retry for {0}ms.", delay);
                    await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
                }
            }

            stoppingToken.ThrowIfCancellationRequested();
            Logger.LogInformation("Successfully connected.");
        }

        private async Task InternalRunAsync(Uri uri, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var stopHeartbeater = true;
                await InternalConnectAsync(uri, stoppingToken).ConfigureAwait(false);
                try
                {
                    var breakReceive = false;
                    var resuming = false;
                    while (!stoppingToken.IsCancellationRequested && !breakReceive)
                    {
                        var payload = await Gateway.ReceiveAsync(stoppingToken).ConfigureAwait(false);
                        Logger.LogTrace("Received payload: {0}.", payload.Op != GatewayPayloadOperation.Dispatch ? payload.Op : payload.T);

                        switch (payload.Op)
                        {
                            case GatewayPayloadOperation.Dispatch:
                            {
                                Sequence = payload.S;
                                switch (payload.T)
                                {
                                    case "READY":
                                    {
                                        Logger.LogInformation("Successfully identified. The gateway is ready.");
                                        // LINQ is faster here as we avoid double ToType()ing (later in the dispatch handler).
                                        SessionId = (string) ((payload.D as IJsonObject)["session_id"] as IJsonValue).Value;
                                        Logger.LogTrace("Session ID: {0}.", SessionId);
                                        break;
                                    }
                                    case "RESUMED":
                                    {
                                        resuming = false;
                                        Logger.LogInformation("The gateway resumed the session.");
                                        break;
                                    }
                                }

                                var e = new GatewayDispatchReceivedEventArgs(payload.T, payload.D);
                                await _dispatchReceivedEvent.InvokeAsync(this, e).ConfigureAwait(false);
                                break;
                            }
                            case GatewayPayloadOperation.Heartbeat:
                            {
                                Logger.LogDebug("The gateway requested a heartbeat.");
                                await Heartbeater.HeartbeatAsync(stoppingToken).ConfigureAwait(false);
                                break;
                            }
                            case GatewayPayloadOperation.Reconnect:
                            {
                                Logger.LogInformation("The gateway requested a reconnect.");
                                stopHeartbeater = false;
                                try
                                {
                                    Logger.LogDebug("Stopping the heartbeater due to a reconnect request.");
                                    await Heartbeater.StopAsync().ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "An exception occurred while stopping the heartbeater.");
                                }

                                try
                                {
                                    Logger.LogInformation("Closing the connection.");
                                    await Gateway.CloseAsync(4000, "Manual close after a requested reconnect.", default).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "An exception occurred while closing the gateway connection.");
                                }

                                breakReceive = true;
                                break;
                            }
                            case GatewayPayloadOperation.InvalidSession:
                            {
                                Logger.LogWarning("The gateway invalidated the session.");
                                if (resuming)
                                {
                                    resuming = false;
                                    var delay = _random.Next(1000, 5001);
                                    Logger.LogInformation("Currently resuming, will start a new session in {0}ms.", delay);
                                    await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
                                    await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                                }
                                else
                                {
                                    if (payload.D.ToType<bool>())
                                    {
                                        resuming = true;
                                        Logger.LogInformation("The session is resumable, resuming...");
                                        await ResumeAsync(stoppingToken).ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        SessionId = null;
                                        Logger.LogInformation("The session is not resumable, identifying...");
                                        await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                                    }
                                }

                                break;
                            }
                            case GatewayPayloadOperation.Hello:
                            {
                                Logger.LogInformation("The gateway said hello.");
                                try
                                {
                                    var model = payload.D.ToType<HelloJsonModel>();
                                    var interval = TimeSpan.FromMilliseconds(model.HeartbeatInterval);
                                    await Heartbeater.StartAsync(interval).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "An exception occurred while starting the heartbeater.");
                                }

                                if (SessionId == null)
                                {
                                    Logger.LogInformation("Identifying...");
                                    await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                                }
                                else
                                {
                                    resuming = true;
                                    Logger.LogInformation("Resuming...");
                                    await ResumeAsync(stoppingToken).ConfigureAwait(false);
                                }

                                break;
                            }
                            case GatewayPayloadOperation.HeartbeatAcknowledged:
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
                            default:
                            {
                                Logger.LogWarning("Unknown gateway operation: {0}.", payload.Op);
                                break;
                            }
                        }
                    }
                }
                catch (WebSocketClosedException ex)
                {
                    if (ex.CloseStatus != null)
                    {
                        var closeCode = (GatewayCloseCode) ex.CloseStatus.Value;
                        switch (closeCode)
                        {
                            case GatewayCloseCode.InvalidSequence:
                                SessionId = null;
                                break;

                            case GatewayCloseCode.AuthenticationFailed:
                            case GatewayCloseCode.InvalidShard:
                            case GatewayCloseCode.ShardingRequired:
                            case GatewayCloseCode.InvalidApiVersion:
                            case GatewayCloseCode.InvalidIntents:
                            case GatewayCloseCode.DisallowedIntents:
                                Logger.LogCritical("The gateway was closed with code {0} indicating a non-recoverable connection - stopping.", closeCode);
                                throw;

                            default:
                                Logger.LogWarning("The gateway was closed with code {0} and reason '{1}'.", closeCode, ex.CloseMessage);
                                break;
                        }
                    }
                    else
                    {
                        Logger.LogWarning(ex, "The gateway was closed with an exception.");
                    }
                }
                catch (TaskCanceledException)
                {
                    Logger.LogInformation("The gateway run was cancelled.");
                    try
                    {
                        await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An exception occurred when closing the gateway connection after cancellation.");
                    }

                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "An exception occurred while receiving a payload. Stopping the connection.");
                    try
                    {
                        await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                    }
                    catch (Exception exc)
                    {
                        Logger.LogError(exc, "An exception occurred when closing the gateway connection after cancellation.");
                    }

                    throw;
                }
                finally
                {
                    if (stopHeartbeater)
                    {
                        try
                        {
                            Logger.LogDebug("Stopping the heartbeater due to connection end.");
                            await Heartbeater.StopAsync().ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, "An exception occurred when stopping the heartbeater.");
                        }
                    }
                }
            }

            SessionId = null;
        }

        private Task IdentifyAsync(CancellationToken cancellationToken)
            => SendAsync(new GatewayPayloadJsonModel
            {
                Op = GatewayPayloadOperation.Identify,
                D = new IdentifyJsonModel
                {
                    Token = Token.RawValue,
                    Properties = new IdentifyJsonModel.PropertiesJsonModel
                    {
                        Os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                            ? "windows"
                            : "unix",
                        Device = "Disqord",
                        Browser = "Disqord"
                    },
                    Intents = Intents,
                    LargeThreshold = LargeThreshold,
                    Shard = Id.Count > 1
                        ? new int[] { Id.Id, Id.Count }
                        : Optional<int[]>.Empty,
                    Presence = Presence
                }
            }, cancellationToken);

        private Task ResumeAsync(CancellationToken cancellationToken)
            => SendAsync(new GatewayPayloadJsonModel
            {
                Op = GatewayPayloadOperation.Resume,
                D = new ResumeJsonModel
                {
                    Token = Token.RawValue,
                    SessionId = SessionId,
                    Seq = Sequence
                }
            }, cancellationToken);

        public void Dispose()
        {
            RateLimiter.Dispose();
            Heartbeater.Dispose();
            Gateway.Dispose();
        }
    }
}
