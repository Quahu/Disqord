using Disqord.Gateway.Api;

namespace Disqord;

public class LocalDiscordShardCoordinatorConfiguration
{
    /// <summary>
    ///     Gets or sets the shard set the coordinator should use
    ///     instead of the Discord's recommended shard count.
    /// </summary>
    public ShardSet? CustomShardSet { get; set; }
}
