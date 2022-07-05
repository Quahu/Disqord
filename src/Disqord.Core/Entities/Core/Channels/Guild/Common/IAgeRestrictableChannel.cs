namespace Disqord;

public interface IAgeRestrictableChannel : ICategorizableGuildChannel
{
    /// <summary>
    ///     Gets whether this channel is age restricted.
    /// </summary>
    bool IsAgeRestricted { get; }
}
