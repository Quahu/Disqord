using System.Collections.Generic;
using Disqord.Gateway.Api;

namespace Disqord.Sharding;

public class DiscordClientSharderConfiguration : IDiscordClientSharderConfiguration
{
    /// <inheritdoc/>
    public int? ShardCount { get; set; }

    /// <inheritdoc/>
    public IEnumerable<ShardId>? ShardIds { get; set; }
}
