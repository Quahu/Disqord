﻿using System;
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
    public int LargeThreshold { get; }

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
    public GatewayState State { get; private set; }

    /// <inheritdoc/>
    public CancellationToken StoppingToken { get; private set; }

    private Tcs _readyTcs;

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
        LargeThreshold = configuration.LargeThreshold;
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

        _readyTcs = new Tcs();
    }

    /// <inheritdoc/>
    public async Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(payload);

        await RateLimiter.WaitAsync(payload.Op, cancellationToken).ConfigureAwait(false);
        try
        {
            Logger.LogTrace("Sending payload: {0}.", payload.Op);
            await Gateway.SendAsync(payload, cancellationToken).ConfigureAwait(false);
            RateLimiter.NotifyCompletion(payload.Op);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while sending payload: {0}.", payload.Op);
            RateLimiter.Release(payload.Op);
            throw;
        }
    }

    /// <inheritdoc/>
    public Task WaitForReadyAsync()
    {
        return _readyTcs.Task;
    }

    /// <inheritdoc/>
    public Task RunAsync(Uri uri, CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        return InternalRunAsync(uri, stoppingToken);
    }

    private async Task InternalConnectAsync(Uri uri, CancellationToken stoppingToken)
    {
        await SetStateAsync(GatewayState.Connecting, stoppingToken).ConfigureAwait(false);

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
        await SetStateAsync(GatewayState.Connected, stoppingToken);
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
                                    await SetStateAsync(GatewayState.Ready, stoppingToken).ConfigureAwait(false);

                                    _readyTcs.Complete();
                                    _readyTcs = new Tcs();
                                    Logger.LogInformation("Successfully identified. The gateway is ready.");
                                    RateLimiter.Reset();

                                    // LINQ is faster here as we avoid double ToType()ing (later in the dispatch handler).
                                    SessionId = ((payload.D as IJsonObject)!["session_id"]! as IJsonValue)!.Value as string;
                                    Logger.LogTrace("Session ID: {0}.", SessionId);
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
                                    await SetStateAsync(GatewayState.Ready, stoppingToken).ConfigureAwait(false);

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
                            await SetStateAsync(GatewayState.Reconnecting, stoppingToken).ConfigureAwait(false);

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
                                var delay = Random.Shared.Next(1000, 5001);
                                Logger.LogInformation("Currently resuming, will start a new session in {0}ms.", delay);
                                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
                                await SetStateAsync(GatewayState.Identifying, stoppingToken).ConfigureAwait(false);
                                await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                            }
                            else
                            {
                                if (payload.D!.ToType<bool>())
                                {
                                    await SetStateAsync(GatewayState.Resuming, stoppingToken).ConfigureAwait(false);

                                    resuming = true;
                                    Logger.LogInformation("The session is resumable, resuming...");
                                    await ResumeAsync(stoppingToken).ConfigureAwait(false);
                                }
                                else
                                {
                                    await SetStateAsync(GatewayState.Identifying, stoppingToken).ConfigureAwait(false);

                                    SessionId = null;
                                    Logger.LogInformation("The session is not resumable, identifying...");
                                    await IdentifyAsync(stoppingToken).ConfigureAwait(false);
                                }
                            }

                            break;
                        }
                        case GatewayPayloadOperation.Hello:
                        {
                            await SetStateAsync(GatewayState.Identifying, stoppingToken).ConfigureAwait(false);

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
                    }

                    switch (closeCode)
                    {
                        case GatewayCloseCode.InvalidSequence:
                        {
                            SessionId = null;
                            break;
                        }
                        case GatewayCloseCode.ShardingRequired when ApiClient.ShardCoordinator.HasDynamicShardSets:
                        {
                            Logger.LogWarning("The gateway was closed with code {0} indicating a new set of shards should be used.", closeCode);
                            throw;
                        }
                        case GatewayCloseCode.AuthenticationFailed:
                        case GatewayCloseCode.InvalidShard:
                        case GatewayCloseCode.ShardingRequired:
                        case GatewayCloseCode.InvalidApiVersion:
                        case GatewayCloseCode.InvalidIntents:
                        case GatewayCloseCode.DisallowedIntents:
                        {
                            Logger.LogCritical("The gateway was closed with code {0} indicating a non-recoverable connection - stopping.", closeCode);
                            throw;
                        }
                        default:
                        {
                            Logger.LogWarning("The gateway was closed with code {0} and reason '{1}'.", closeCode, ex.CloseMessage);
                            break;
                        }
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
                await SetStateAsync(GatewayState.Disconnected, stoppingToken).ConfigureAwait(false);

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
    }

    private async ValueTask SetStateAsync(GatewayState newState, CancellationToken stoppingToken)
    {
        var oldState = State;
        if (oldState == newState)
        {
            Debug.Fail("redundant state change");
            return;
        }

        State = newState;
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
                LargeThreshold = LargeThreshold,
                Shard = Id.Count > 1
                    ? new[] { Id.Index, Id.Count }
                    : Optional<int[]>.Empty,
                Presence = Optional.FromNullable(Presence)
            }
        }, stoppingToken).ConfigureAwait(false);
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

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return Gateway.DisposeAsync();
    }
}
