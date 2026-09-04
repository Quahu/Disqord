namespace Disqord.Gateway;

/// <summary>
///     Defines how guilds are chunked when automatic member chunking is enabled.
/// </summary>
public enum AutomaticMemberChunkingStrategy : byte
{
    /// <summary>
    ///     Each guild is chunked as soon as it becomes available,
    ///     without waiting for the remaining guilds to be received.
    /// </summary>
    AsAvailable = 0,

    /// <summary>
    ///     All guilds are received first and then chunked in the order
    ///     returned by the <see cref="IAutomaticGatewayMemberChunkingSelector"/>.
    /// </summary>
    WhenAllAvailable = 1,
}
