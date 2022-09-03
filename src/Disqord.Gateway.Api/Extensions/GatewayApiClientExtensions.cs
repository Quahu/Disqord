using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Disqord.Gateway.Api;
using Qommon;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class GatewayApiClientExtensions
{
    public static ShardId GetShardId(this IGatewayApiClient apiClient, Snowflake? guildId)
    {
        Guard.HasSizeGreaterThan(apiClient.Shards, 0);

        return ShardId.FromGuildId(guildId ?? 0, apiClient.Shards.First().Key.Count);
    }

    public static IShard? GetShard(this IGatewayApiClient apiClient, Snowflake? guildId)
    {
        return apiClient.Shards.GetValueOrDefault(apiClient.GetShardId(guildId));
    }
}
