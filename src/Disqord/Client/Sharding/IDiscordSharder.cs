using System.Collections.Generic;
using Qommon.Events;

namespace Disqord.Sharding
{
    public interface IDiscordSharder
    {
        event AsynchronousEventHandler<ShardReadyEventArgs> ShardReady;

        internal AsynchronousEvent<ShardReadyEventArgs> _shardReady { get; set; }

        IReadOnlyList<Shard> Shards { get; }

        int GetShardId(Snowflake guildId);
    }
}
