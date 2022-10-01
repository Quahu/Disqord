using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Gateway;

/// <summary>
///     Represents a member chunker responsible for handling requests
///     and responses for <see cref="GatewayPayloadOperation.RequestMembers"/>.
/// </summary>
public interface IGatewayChunker : IBindable<IGatewayClient>, ILogging
{
    /// <summary>
    ///     Gets the gateway client this chunker is bound to.
    /// </summary>
    IGatewayClient Client { get; }

    /// <summary>
    ///     Called when a chunk is received.
    /// </summary>
    /// <param name="model"> The chunk model. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    ValueTask OnChunk(GuildMembersChunkJsonModel model);

    /// <summary>
    ///     Chunks the specified guild,
    ///     i.e. queries all members of the guild.
    /// </summary>
    /// <param name="guild"> The guild to chunk. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the operation
    ///     with the result indicating whether the operation was actually performed.
    /// </returns>
    ValueTask<bool> ChunkAsync(IGatewayGuild guild, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Queries up to <see cref="Discord.Limits.Gateway.QueryMembersLimit"/> members in the guild with the given ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild to query the members in. </param>
    /// <param name="query"> The text that the names of the members should start with. </param>
    /// <param name="limit"> The amount of matching members to be returned. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the operation
    ///     with the result being the queried members keyed by their IDs.
    /// </returns>
    ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, string query, int limit = Discord.Limits.Gateway.QueryMembersLimit, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Queries up to <see cref="Discord.Limits.Gateway.QueryMembersLimit"/> members in the guild with the given ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild to query the members in. </param>
    /// <param name="memberIds"> The IDs of the members to query. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the operation
    ///     with the result being the queried members keyed by their IDs.
    /// </returns>
    ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, IEnumerable<Snowflake> memberIds, CancellationToken cancellationToken = default);
}
