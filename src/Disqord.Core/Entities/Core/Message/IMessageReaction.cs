namespace Disqord;

/// <summary>
///     Represents a message reaction,
///     i.e. a "clickable" emoji under the message within the desktop client.
/// </summary>
public interface IMessageReaction : IEntity
{
    /// <summary>
    ///     Gets the emoji of this reaction.
    /// </summary>
    IEmoji Emoji { get; }

    /// <summary>
    ///     Gets the amount of users that reacted with the emoji.
    /// </summary>
    int Count { get; }

    /// <summary>
    ///     Gets whether the bot has reacted with the emoji.
    /// </summary>
    bool HasOwnReaction { get; }
}
