using System;
using System.Collections.Generic;
using Disqord.Gateway.Api;
using Qommon;

namespace Disqord.Gateway;

public class ReadyEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the shard that became ready.
    /// </summary>
    public ShardId ShardId { get; }

    /// <summary>
    ///     Gets the IDs of the guilds of the ready shard.
    /// </summary>
    public IReadOnlyList<Snowflake> GuildIds { get; }

    /// <summary>
    ///     Gets the current user, i.e. the identified bot.
    /// </summary>
    public ICurrentUser CurrentUser { get; }

    /// <summary>
    ///     Gets the ID of the current application.
    /// </summary>
    public Snowflake CurrentApplicationId { get; }

    /// <summary>
    ///     Gets the flags of the current application.
    /// </summary>
    public ApplicationFlags CurrentApplicationFlags { get; }

    public ReadyEventArgs(
        ShardId shardId,
        IReadOnlyList<Snowflake> guildIds,
        ICurrentUser currentUser,
        Snowflake currentApplicationId,
        ApplicationFlags currentApplicationFlags)
    {
        Guard.IsNotNull(guildIds);
        Guard.IsNotNull(currentUser);

        ShardId = shardId;
        GuildIds = guildIds;
        CurrentUser = currentUser;
        CurrentApplicationId = currentApplicationId;
        CurrentApplicationFlags = currentApplicationFlags;
    }
}
