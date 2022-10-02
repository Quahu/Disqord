using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Qommon.Binding;

namespace Disqord.Api;

/// <summary>
///     Represents an <see cref="IShardCoordinator"/> which is bound to a <see cref="DiscordClientBase"/>.
/// </summary>
/// <remarks>
///     Implement this type rather than <see cref="IShardCoordinator"/> directly.
/// </remarks>
public abstract class DiscordShardCoordinator : IShardCoordinator, IBindable<DiscordClientBase>
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <summary>
    ///     Gets the client of this coordinator.
    /// </summary>
    /// <returns>
    ///     The client or <b><see langword="null"/> if accessed in the constructor</b>.
    /// </returns>
    public DiscordClientBase Client => _binder.Value;

    /// <inheritdoc/>
    public abstract bool HasDynamicShardSets { get; }

    /// <summary>
    ///     Gets or sets the current shard set.
    /// </summary>
    /// <remarks>
    ///     Defaults to an empty instance.
    /// </remarks>
    protected ShardSet CurrentShardSet { get; set; }

    private readonly Binder<DiscordClientBase> _binder;

    protected DiscordShardCoordinator(
        ILogger logger)
    {
        Logger = logger;

        _binder = new(this, allowRebinding: true);
    }

    /// <inheritdoc/>
    public virtual void Bind(DiscordClientBase value)
    {
        _binder.Bind(value);
    }

    /// <summary>
    ///     Invoked when a new <see cref="ShardSet"/> is requested.
    /// </summary>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the callback work
    ///     with the result being the <see cref="ShardSet"/>.
    /// </returns>
    protected virtual async ValueTask<ShardSet> OnGetShardSet(CancellationToken stoppingToken)
    {
        var gatewayData = await Client.FetchBotGatewayDataAsync(cancellationToken: stoppingToken).ConfigureAwait(false);
        var shardCount = gatewayData.RecommendedShardCount;
        var maxConcurrency = gatewayData.Sessions.MaxConcurrency;
        return ShardSet.FromCount(shardCount, maxConcurrency);
    }

    /// <inheritdoc/>
    public async ValueTask<ShardSet> GetShardSetAsync(CancellationToken stoppingToken)
    {
        var currentShardSet = CurrentShardSet;
        if (!currentShardSet.Equals(default))
            return currentShardSet;

        currentShardSet = await OnGetShardSet(stoppingToken).ConfigureAwait(false);
        CurrentShardSet = currentShardSet;
        return currentShardSet;
    }

    /// <inheritdoc/>
    public virtual ValueTask OnShardSetInvalidated(CancellationToken stoppingToken)
    {
        CurrentShardSet = default;
        return default;
    }

    /// <inheritdoc/>
    public abstract ValueTask WaitToIdentifyShardAsync(ShardId shardId, CancellationToken stoppingToken);

    /// <inheritdoc/>
    public virtual ValueTask OnShardIdentifySent(ShardId shardId, CancellationToken stoppingToken)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask OnShardReady(ShardId shardId, string sessionId, CancellationToken stoppingToken)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask OnShardSessionInvalidated(ShardId shardId, string? sessionId, CancellationToken stoppingToken)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask OnShardDisconnected(ShardId shardId, Exception? exception, string? sessionId, CancellationToken stoppingToken)
    {
        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask OnShardStateUpdated(ShardId shardId, ShardState oldState, ShardState newState, CancellationToken stoppingToken)
    {
        return default;
    }
}
