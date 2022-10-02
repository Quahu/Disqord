using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Disqord.Utilities.Threading;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;

namespace Disqord.Gateway.Api.Default;

public class DefaultShard : IShard
{
    /// <inheritdoc/>
    public ShardId Id { get; }

    /// <inheritdoc/>
    public GatewayIntents Intents { get; }

    /// <inheritdoc/>
    public int LargeGuildThreshold { get; }

    /// <inheritdoc/>
    public UpdatePresenceJsonModel? Presence { get; set; }

    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public IGatewayApiClient ApiClient { get; }

    /// <inheritdoc/>
    public IJsonSerializer Serializer { get; }

    /// <inheritdoc/>
    public IGatewayRateLimiter RateLimiter { get; }

    /// <inheritdoc/>
    public IGatewayHeartbeater Heartbeater { get; }

    /// <inheritdoc/>
    public IGateway Gateway { get; }

    /// <inheritdoc/>
    /// <inheritdoc/>
    public string? SessionId { get; private set; }

    /// <inheritdoc/>
    public int? Sequence { get; private set; }

    /// <inheritdoc/>
    public Uri? ResumeUri { get; private set; }

    /// <inheritdoc/>
    public ShardState State { get; private set; }

    /// <inheritdoc/>
    public CancellationToken StoppingToken { get; private set; }

    private readonly object _stateLock = new();
    private Tcs? _readyTcs;

    public DefaultShard(
        ShardId id,
        IOptions<DefaultShardConfiguration> options,
        ILoggerFactory loggerFactory,
        IGatewayApiClient apiClient,
        IGatewayRateLimiter rateLimiter,
        IGatewayHeartbeater heartbeater,
        IGateway gateway)
    {
        Id = id;
        var configuration = options.Value;
        Intents = configuration.Intents;
        LargeGuildThreshold = configuration.LargeGuildThreshold;
        Presence = configuration.Presence;
        Logger = loggerFactory.CreateLogger($"Shard #{id.Index}");
        ApiClient = apiClient;
        Serializer = apiClient.Serializer;
        RateLimiter = rateLimiter;
        rateLimiter.Bind(this);
        Heartbeater = heartbeater;
        heartbeater.Bind(this);
        Gateway = gateway;
        gateway.Bind(this);
    }

    /// <inheritdoc/>
    public async Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(payload);

        var sent = false;
        do
        {
            await RateLimiter.WaitAsync(payload.Op, cancellationToken).ConfigureAwait(false);
            try
            {
                Logger.LogTrace("Sending payload: {0}.", payload.Op);
                await Gateway.SendAsync(payload, cancellationToken).ConfigureAwait(false);
                sent = true;
                RateLimiter.NotifyCompletion(payload.Op);
            }
            catch (WebSocketClosedException ex) when (ex.CloseStatus != null && ((GatewayCloseCode) ex.CloseStatus).IsRecoverable())
            {
                await WaitForReadyAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    Logger.LogError(ex, "An exception occurred while sending payload: {0}.", payload.Op);

                RateLimiter.Release(payload.Op);
                throw;
            }
        }
        while (!sent);
    }

    /// <inheritdoc/>
    public Task WaitForReadyAsync(CancellationToken cancellationToken)
    {
        lock (_stateLock)
        {
            if (State == ShardState.Ready)
                return Task.CompletedTask;

            return (_readyTcs ??= new()).Task.WaitAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public Task RunAsync(Uri? initialUri, CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        return InternalRunAsync(initialUri, stoppingToken);
    }

    private async Task InternalConnectAsync(Uri? uri, CancellationToken stoppingToken)
    {
        await SetStateAsync(ShardState.Connecting, stoppingToken).ConfigureAwait(false);

        var attempt = 0;
        var delay = 2500;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (attempt == 0)
                {
                    Logger.LogDebug("Connecting to the gateway...");
                }
                else
                {
                    Logger.LogDebug("Retrying (attempt {Attempt}) connecting to the gateway...", attempt + 1);
                }

                await Gateway.ConnectAsync(uri ?? new Uri(Discord.Gateway.DefaultUrl), stoppingToken).ConfigureAwait(false);
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
        await SetStateAsync(ShardState.Connected, stoppingToken).ConfigureAwait(false);
    }

    private async Task InternalRunAsync(Uri? uri, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var stopHeartbeater = true;
            await InternalConnectAsync(ResumeUri ?? uri, stoppingToken).ConfigureAwait(false);
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
                                    await SetStateAsync(ShardState.Ready, stoppingToken).ConfigureAwait(false);

                                    Logger.LogInformation("Successfully identified. The gateway is ready.");
                                    RateLimiter.Reset();

                                    // LINQ is faster here as we avoid double ToType()ing (later in the dispatch handler).
                                    var d = (payload.D as IJsonObject)!;
                                    SessionId = (d["session_id"] as IJsonValue)!.Value as string;
                                    var resumeGatewayUrl = (d["resume_gateway_url"] as IJsonValue)!.Value as string;
                                    if (resumeGatewayUrl != null)
                                    {
                                        ResumeUri = new Uri(resumeGatewayUrl);
                                    }

                                    Logger.LogTrace("Session ID: {SessionId}, Resume URI: {ResumeUri}.", SessionId, ResumeUri);

                                    lock (_stateLock)
                                    {
                                        _readyTcs?.Complete();
                                        _readyTcs = null;
                                    }

                                    try
                                    {
                                        await ApiClient.ShardCoordinator.OnShardReady(Id, SessionId!, stoppingToken).ConfigureAwait(false);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogError(ex, "An exception occurred while invoking {CoordinatorMethodName} on the coordinator.", nameof(IShardCoordinator.OnShardReady));
                                    }

                                    break;
                                }
                                case "RESUMED":
                                {
                                    await SetStateAsync(ShardState.Ready, stoppingToken).ConfigureAwait(false);

                                    resuming = false;
                                    Logger.LogInformation("The gateway resumed the session.");
                                    break;
                                }
                            }

                            var e = new GatewayDispatchReceivedEventArgs(payload.T!, payload.D!);
                            await ApiClient.DispatchReceivedEvent.InvokeAsync(this, e).ConfigureAwait(false);
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
                            await SetStateAsync(ShardState.Reconnecting, stoppingToken).ConfigureAwait(false);

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
                            await ApiClient.ShardCoordinator.OnShardSessionInvalidated(Id, SessionId, stoppingToken).ConfigureAwait(false);

                            if (resuming)
                            {
                                resuming = false;
                                var delay = Random.Shared.Next(1000, 5001);
                                Logger.LogInformation("The gateway did not resume the session, identifying in {0}ms...", delay);
                                await SetStateAsync(ShardState.Identifying, stoppingToken).ConfigureAwait(false);

                                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
                                await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                            }
                            else
                            {
                                var isResumable = payload.D!.ToType<bool>();
                                if (isResumable)
                                {
                                    resuming = true;
                                    Logger.LogInformation("The gateway invalidated the session (resumable), resuming...");
                                    await SetStateAsync(ShardState.Resuming, stoppingToken).ConfigureAwait(false);
                                    await ResumeAsync(stoppingToken).ConfigureAwait(false);
                                }
                                else
                                {
                                    if (State == ShardState.Identifying)
                                    {
                                        Logger.LogWarning("Hit the identify rate-limit, retrying...");
                                    }
                                    else
                                    {
                                        SessionId = null;
                                        ResumeUri = null;
                                        Logger.LogInformation("The gateway invalidated the session (not resumable), identifying...");
                                        await SetStateAsync(ShardState.Identifying, stoppingToken).ConfigureAwait(false);
                                    }

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
                                var model = payload.D!.ToType<HelloJsonModel>()!;
                                var interval = TimeSpan.FromMilliseconds(model.HeartbeatInterval);
                                await Heartbeater.StartAsync(interval, stoppingToken).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "An exception occurred while starting the heartbeater.");
                            }

                            if (SessionId == null)
                            {
                                await SetStateAsync(ShardState.Identifying, stoppingToken).ConfigureAwait(false);

                                Logger.LogInformation("Identifying...");
                                await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                            }
                            else
                            {
                                await SetStateAsync(ShardState.Resuming, stoppingToken).ConfigureAwait(false);

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
                    if (closeCode.IsRecoverable())
                    {
                        if (closeCode == GatewayCloseCode.InvalidSequence)
                        {
                            // Can't happen with default Disqord components.
                            SessionId = null;
                        }

                        Logger.LogWarning("The gateway was closed with code {0} and reason '{1}'.", closeCode, ex.CloseMessage);
                    }
                    else
                    {
                        if (closeCode == GatewayCloseCode.ShardingRequired)
                        {
                            try
                            {
                                await ApiClient.ShardCoordinator.OnShardSetInvalidated(stoppingToken).ConfigureAwait(false);
                            }
                            catch (Exception exc)
                            {
                                Logger.LogError(exc, "An exception occurred while invoking {CoordinatorMethodName} on the coordinator.", nameof(IShardCoordinator.OnShardSetInvalidated));
                            }

                            if (ApiClient.ShardCoordinator.HasDynamicShardSets)
                            {
                                Logger.LogWarning("The gateway was closed with code {0} indicating a new set of shards should be used.", closeCode);
                                throw;
                            }
                        }

                        Logger.LogCritical("The gateway was closed with code {0} indicating a non-recoverable connection - stopping.", closeCode);
                        throw;
                    }
                }
                else
                {
                    Logger.LogWarning(ex, "The gateway was closed with an exception.");
                }

                await OnDisconnected(ex, stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogInformation("The gateway run was cancelled.");

                try
                {
                    await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                    await OnDisconnected(ex, stoppingToken).ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exc, "An exception occurred while closing the gateway connection after cancellation.");
                }

                throw;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "An exception occurred while receiving a payload. Stopping the connection.");
                try
                {
                    await Gateway.CloseAsync(1000, null, default).ConfigureAwait(false);
                    await OnDisconnected(ex, stoppingToken).ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exc, "An exception occurred while closing the gateway connection after cancellation.");
                }

                throw;
            }
            finally
            {
                await SetStateAsync(ShardState.Disconnected, stoppingToken).ConfigureAwait(false);

                if (stopHeartbeater)
                {
                    try
                    {
                        Logger.LogDebug("Stopping the heartbeater due to connection end.");
                        await Heartbeater.StopAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An exception occurred while stopping the heartbeater.");
                    }
                }
            }
        }

        SessionId = null;
        ResumeUri = null;
    }

    private async ValueTask SetStateAsync(ShardState newState, CancellationToken stoppingToken)
    {
        ShardState oldState;
        lock (_stateLock)
        {
            oldState = State;
            if (oldState == newState)
            {
                Debug.Fail("redundant state change");
                return;
            }

            State = newState;
        }

        try
        {
            await ApiClient.ShardCoordinator.OnShardStateUpdated(Id, oldState, newState, stoppingToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while notifying the shard coordinator of the state change ({OldState} -> {NewState}).", oldState, newState);
        }
    }

    private async ValueTask OnDisconnected(Exception? ex, CancellationToken stoppingToken)
    {
        try
        {
            await ApiClient.ShardCoordinator.OnShardDisconnected(Id, ex, SessionId, stoppingToken).ConfigureAwait(false);
        }
        catch (Exception exc)
        {
            Logger.LogError(exc, "An exception occurred while invoking {CoordinatorMethodName} on the coordinator.", nameof(IShardCoordinator.OnShardSetInvalidated));
        }
    }

    private async ValueTask IdentifyAsync(CancellationToken stoppingToken)
    {
        await ApiClient.ShardCoordinator.WaitToIdentifyShardAsync(Id, stoppingToken).ConfigureAwait(false);
        await SendAsync(new GatewayPayloadJsonModel
        {
            Op = GatewayPayloadOperation.Identify,
            D = new IdentifyJsonModel
            {
                Token = ApiClient.Token.RawValue,
                Properties = new IdentifyJsonModel.PropertiesJsonModel
                {
                    Os = OperatingSystem.IsWindows()
                        ? "windows"
                        : "unix",
                    Device = "Disqord",
                    Browser = "Disqord"
                },
                Intents = Intents,
                LargeThreshold = LargeGuildThreshold,
                Shard = Id.Count > 1
                    ? new[] { Id.Index, Id.Count }
                    : Optional<int[]>.Empty,
                Presence = Optional.FromNullable(Presence)
            }
        }, stoppingToken).ConfigureAwait(false);

        await ApiClient.ShardCoordinator.OnShardIdentifySent(Id, stoppingToken);
    }

    private Task ResumeAsync(CancellationToken stoppingToken)
    {
        return SendAsync(new GatewayPayloadJsonModel
        {
            Op = GatewayPayloadOperation.Resume,
            D = new ResumeJsonModel
            {
                Token = ApiClient.Token.RawValue,
                SessionId = SessionId!,
                Seq = Sequence
            }
        }, stoppingToken);
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return Gateway.DisposeAsync();
    }
}
