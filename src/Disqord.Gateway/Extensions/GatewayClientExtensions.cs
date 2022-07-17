using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Gateway.Api;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class GatewayClientExtensions
{
    public static ShardId GetShardId(this IGatewayClient client, Snowflake? guildId)
    {
        return ShardId.ForGuildId(guildId ?? 0, client.Shards.Count);
    }

    public static IGatewayApiClient? GetShard(this IGatewayClient client, Snowflake? guildId)
    {
        return client.Shards.GetValueOrDefault(client.GetShardId(guildId));
    }
}
