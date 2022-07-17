namespace Disqord;

/// <summary>
///     Represents a guild channel that might be age-restricted.
/// </summary>
public interface IAgeRestrictableChannel : ICategorizableGuildChannel
{
    /// <summary>
    ///     Gets whether this channel is age-restricted.
    /// </summary>
    bool IsAgeRestricted { get; }
}