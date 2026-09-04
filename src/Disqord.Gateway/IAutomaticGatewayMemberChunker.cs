using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;

namespace Disqord.Gateway;

/// <summary>
///     Represents the notifications an <see cref="IGatewayChunker"/> receives to chunk guilds on its own as they become available.
/// </summary>
public interface IAutomaticGatewayMemberChunker
{
    /// <summary>
    ///     Gets whether automatic member chunking is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    ///     Notifies this chunker that the given shard started a new session,
    ///     cancelling the in-progress automatic chunking of the previous session.
    /// </summary>
    /// <param name="shard"> The shard that started the session. </param>
    /// <param name="guildIds"> The IDs of the guilds of the session. </param>
    /// <returns>
    ///     A <see cref="Task"/> that completes when the pending guilds of the session are chunked.
    /// </returns>
    Task OnReady(IShard shard, IReadOnlyList<Snowflake> guildIds);

    /// <summary>
    ///     Notifies this chunker that a guild has become available.
    /// </summary>
    /// <param name="shard"> The shard the guild belongs to. </param>
    /// <param name="guild"> The guild that became available. </param>
    /// <param name="isPending"> Whether the guild is one of the shard's pending guilds. </param>
    void OnGuildAvailable(IShard shard, IGatewayGuild guild, bool isPending);

    /// <summary>
    ///     Notifies this chunker that the given shard has received all of its pending guilds.
    /// </summary>
    /// <param name="shardId"> The ID of the shard that received its guilds. </param>
    void OnPendingGuildsReceived(ShardId shardId);
}
