using System;
using Disqord.Gateway.Api;

namespace Disqord;

public class LocalDiscordShardCoordinatorConfiguration
{
    /// <summary>
    ///     Gets or sets the shard set the coordinator should use
    ///     instead of the Discord's recommended shard count.
    /// </summary>
    public ShardSet? CustomShardSet { get; set; }

    /// <summary>
    ///     Gets or sets the identify delay used between separate identify calls.
    /// </summary>
    /// <remarks>
    ///     Defaults to <c>5.25</c> seconds.
    /// </remarks>
    public TimeSpan IdentifyDelay { get; set; } = TimeSpan.FromSeconds(5.25);
}
