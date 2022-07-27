using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents the default, local shard coordinator implementation.
/// </summary>
public class LocalDiscordShardCoordinator : DiscordShardCoordinator
{
    /// <inheritdoc/>
    /// <returns>
    ///     <see langword="true"/>
    /// </returns>
    public override bool HasDynamicShardSets => CustomShardset.Equals(default);

    protected ShardSet CurrentShardSet;
    protected ShardSet CustomShardset;

    protected SemaphoreSlim? _semaphore;
    protected Timer? _semaphoreTimer;

    public LocalDiscordShardCoordinator(
        IOptions<LocalDiscordShardCoordinatorConfiguration> options,
        ILogger<LocalDiscordShardCoordinator> logger)
        : base(logger)
    {
        var configuration = options.Value;
        var customShardSet = configuration.CustomShardSet;
        CustomShardset = customShardSet.GetValueOrDefault();
    }

    /// <inheritdoc/>
    public override async ValueTask<ShardSet> GetShardSetAsync(CancellationToken stoppingToken)
    {
        var currentShardSet = CurrentShardSet;
        if (!currentShardSet.Equals(default))
            return currentShardSet;

        if (CustomShardset.Equals(default))
        {
            var gatewayData = await Client.FetchBotGatewayDataAsync(cancellationToken: stoppingToken).ConfigureAwait(false);
            var shardCount = gatewayData.RecommendedShardCount;
            var maxConcurrency = gatewayData.Sessions.MaxConcurrency;
            Logger.LogInformation("Using Discord's recommended shard count of {ShardCount}.", shardCount);
            currentShardSet = new ShardSet(shardCount, maxConcurrency);
        }
        else
        {
            currentShardSet = CustomShardset;
            Logger.LogInformation("Using a custom shard count of {ShardCount}.", currentShardSet.ShardIds.Count);
        }

        lock (this)
        {
            CurrentShardSet = currentShardSet;
            _semaphore = new SemaphoreSlim(currentShardSet.MaxConcurrency, currentShardSet.MaxConcurrency);
        }

        return currentShardSet;
    }

    /// <inheritdoc/>
    public override ValueTask WaitToIdentifyShardAsync(ShardId shardId, CancellationToken stoppingToken)
    {
        lock (this)
        {
            var currentShardSet = CurrentShardSet;
            var semaphore = _semaphore;
            Guard.IsNotDefault(currentShardSet);
            Guard.IsNotNull(semaphore);

            return new(semaphore.WaitAsync(stoppingToken));
        }
    }

    /// <inheritdoc/>
    public override ValueTask OnShardSetInvalidated(CancellationToken stoppingToken)
    {
        CurrentShardSet = default;
        return default;
    }

    /// <inheritdoc/>
    public override ValueTask OnShardReady(ShardId shardId, string sessionId, CancellationToken stoppingToken)
    {
        lock (this)
        {
            _semaphoreTimer ??= new Timer(state =>
            {
                var @this = Unsafe.As<LocalDiscordShardCoordinator>(state!);
                try
                {
                    lock (@this)
                    {
                        var releaseCount = @this.CurrentShardSet.MaxConcurrency - @this._semaphore!.CurrentCount;
                        @this._semaphore.Release(releaseCount);
                        @this._semaphoreTimer = null;
                    }
                }
                catch (Exception ex)
                {
                    @this.Logger.LogError(ex, "An exception occurred in the coordinator semaphore timer.");
                }
            }, this, TimeSpan.FromSeconds(5), TimeSpan.Zero);
        }

        return default;
    }

    /// <inheritdoc/>
    public override ValueTask OnShardDisconnected(ShardId shardId, Exception? exception, string? sessionId, CancellationToken stoppingToken)
    {
        return default;
    }

    /// <inheritdoc/>
    public override ValueTask OnShardStateUpdated(ShardId shardId, GatewayState oldState, GatewayState newState, CancellationToken stoppingToken)
    {
        return default;
    }
}
