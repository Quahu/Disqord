using System.Collections.Generic;

namespace Disqord.Sharding
{
    public interface IDiscordSharder
    {
        IReadOnlyList<DiscordSharder.Shard> Shards { get; }

        int GetShardId(Snowflake guildId);
    }
}