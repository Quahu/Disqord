namespace Disqord;

/// <summary>
///     Represents a Discord gateway bot response providing the gateway url,
///     recommended shard count, and gateway session usage data.
/// </summary>
public interface IBotGatewayData : IEntity
{
    /// <summary>
    ///     Gets the gateway URL.
    /// </summary>
    string Url { get; }

    /// <summary>
    ///     Gets the shard count recommended by Discord.
    /// </summary>
    int RecommendedShardCount { get; }

    /// <summary>
    ///     Gets the gateway session data.
    /// </summary>
    IBotGatewaySessionData Sessions { get; }
}