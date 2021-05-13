using System;
using System.Collections.Generic;
using Disqord.Gateway.Api;

namespace Disqord.Gateway
{
    public class ReadyEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the shard that became ready.
        /// </summary>
        public ShardId ShardId { get; }

        /// <summary>
        ///     Gets the current user, i.e. the identified bot.
        /// </summary>
        public ICurrentUser CurrentUser { get; }

        /// <summary>
        ///     Gets the IDs of the guilds this shard will receive.
        /// </summary>
        public IReadOnlyList<Snowflake> GuildIds { get; }

        public ReadyEventArgs(ShardId shardId, ICurrentUser currentUser, IReadOnlyList<Snowflake> guildIds)
        {
            if (currentUser == null)
                throw new ArgumentNullException(nameof(currentUser));

            if (guildIds == null)
                throw new ArgumentNullException(nameof(guildIds));

            ShardId = shardId;
            CurrentUser = currentUser;
            GuildIds = guildIds;
        }
    }
}
