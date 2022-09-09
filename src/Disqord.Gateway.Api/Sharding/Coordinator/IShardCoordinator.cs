using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.WebSocket;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents the type responsible for providing shard sets for the local machine
///     and shard connection coordination.
/// </summary>
public interface IShardCoordinator : ILogging
{
    /// <summary>
    ///     Gets whether the shard sets this coordinator provides are dynamic,
    ///     i.e. whether it can produce a new <see cref="ShardSet"/>
    ///     if the gateway gets closed with <see cref="GatewayCloseCode.ShardingRequired"/>.
    /// </summary>
    bool HasDynamicShardSets { get; }

    /// <summary>
    ///     Gets the shard set of this coordinator.
    /// </summary>
    /// <remarks>
    ///     The result should be cached by the coordinator
    ///     until <see cref="OnShardSetInvalidated"/> is called.
    /// </remarks>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work with the result being the shard set.
    /// </returns>
    ValueTask<ShardSet> GetShardSetAsync(CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked when the shard set is or has become invalid.
    /// </summary>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    ValueTask OnShardSetInvalidated(CancellationToken stoppingToken);

    /// <summary>
    ///     Waits to identify shard with the given ID.
    /// </summary>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the wait.
    /// </returns>
    ValueTask WaitToIdentifyShardAsync(ShardId shardId, CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked after the shard with the given ID sends an identify payload.
    /// </summary>
    /// <remarks>
    ///     Ensure this method does not take a long time to complete
    ///     as it is invoked directly by the shard during its gateway flow.
    /// </remarks>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the wait.
    /// </returns>
    ValueTask OnShardIdentifySent(ShardId shardId, CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked when the shard with the given ID becomes ready,
    ///     i.e. has successfully identified with the gateway.
    /// </summary>
    /// <remarks>
    ///     This is called before the shard's guilds are available.
    ///     <para/>
    ///     Ensure this method does not take a long time to complete
    ///     as it is invoked directly by the shard during its gateway flow.
    /// </remarks>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="sessionId"> The ID of the gateway session. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    ValueTask OnShardReady(ShardId shardId, string sessionId, CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked when the shard with the given ID has its session invalidated.
    /// </summary>
    /// <remarks>
    ///     Ensure this method does not take a long time to complete
    ///     as it is invoked directly by the shard during its gateway flow.
    /// </remarks>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="sessionId"> The ID of the gateway session. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the wait.
    /// </returns>
    ValueTask OnShardSessionInvalidated(ShardId shardId, string? sessionId, CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked when the shard with the given ID disconnects.
    /// </summary>
    /// <remarks>
    ///     Ensure this method does not take a long time to complete
    ///     as it is invoked directly by the shard during its gateway flow.
    /// </remarks>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="exception">
    ///     The exception that caused the disconnect.
    ///     If the exception is not a <see cref="WebSocketClosedException"/> the disconnect was not caused by Discord.
    /// </param>
    /// <param name="sessionId"> The ID of the gateway session, if any. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    ValueTask OnShardDisconnected(ShardId shardId, Exception? exception, string? sessionId, CancellationToken stoppingToken);

    /// <summary>
    ///     Invoked when the state of the shard with the given ID changes.
    /// </summary>
    /// <remarks>
    ///     Ensure this method does not take a long time to complete
    ///     as it is invoked directly by the shard during its gateway flow.
    /// </remarks>
    /// <param name="shardId"> The ID of the shard. </param>
    /// <param name="oldState"> The gateway state before the update. </param>
    /// <param name="newState"> The gateway state after the update. </param>
    /// <param name="stoppingToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    ValueTask OnShardStateUpdated(ShardId shardId, ShardState oldState, ShardState newState, CancellationToken stoppingToken);
}
