namespace Disqord;

/// <summary>
///     Represents a welcome screen's channel.
/// </summary>
public interface IGuildWelcomeScreenChannel : IChannelEntity
{
    /// <summary>
    ///     Gets the description of this welcome screen channel.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the emoji of this welcome screen channel.
    /// </summary>
    IEmoji? Emoji { get; }
}
