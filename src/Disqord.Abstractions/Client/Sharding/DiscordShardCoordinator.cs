using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default;
using Disqord.Gateway.Default.Dispatcher;
using Microsoft.Extensions.Logging;
using Qommon.Binding;

namespace Disqord;

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
    ///     Invoked on client initialization.
    /// </summary>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    protected virtual ValueTask OnInitialize(CancellationToken stoppingToken)
    {
        return default;
    }

    public async ValueTask InitializeAsync(CancellationToken stoppingToken)
    {
        await OnInitialize(stoppingToken);

        // TODO: solve this ready mess?
        if ((Client as IGatewayClient).Dispatcher is DefaultGatewayDispatcher dispatcher && dispatcher["READY"] is ReadyDispatchHandler readyDispatchHandler)
        {
            var shardSet = await GetShardSetAsync(stoppingToken).ConfigureAwait(false);
            foreach (var shardId in shardSet.ShardIds)
            {
                readyDispatchHandler.InitialReadys[shardId] = new();
            }
        }
    }

    /// <inheritdoc/>
    public abstract ValueTask<ShardSet> GetShardSetAsync(CancellationToken stoppingToken);

    /// <inheritdoc/>
    public abstract ValueTask WaitToIdentifyShardAsync(ShardId shardId, CancellationToken stoppingToken);

    /// <inheritdoc/>
    public abstract ValueTask OnShardSetInvalidated(CancellationToken stoppingToken);

    /// <inheritdoc/>
    public abstract ValueTask OnShardReady(ShardId shardId, string sessionId, CancellationToken stoppingToken);

    /// <inheritdoc/>
    public abstract ValueTask OnShardDisconnected(ShardId shardId, Exception? exception, string? sessionId, CancellationToken stoppingToken);

    /// <inheritdoc/>
    public abstract ValueTask OnShardStateUpdated(ShardId shardId, GatewayState oldState, GatewayState newState, CancellationToken stoppingToken);
}
