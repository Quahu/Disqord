using System;
using System.Collections.Generic;
using Disqord.Gateway.Api;

namespace Disqord.Gateway
{
    public class ReadyEventArgs : EventArgs
    {
        public ShardId ShardId { get; }

        public ICurrentUser CurrentUser { get; }

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
