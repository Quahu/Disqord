using System.Collections.Generic;
using Disqord.Gateway.Api;
using Disqord.Hosting;

namespace Disqord.Sharding;

public class DiscordClientSharderHostingContext : DiscordClientHostingContext, IDiscordClientSharderConfiguration
{
    /// <inheritdoc/>
    public int? ShardCount { get; set; }

    /// <inheritdoc/>
    public IEnumerable<ShardId>? ShardIds { get; set; }
}
