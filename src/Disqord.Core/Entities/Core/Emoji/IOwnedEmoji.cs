namespace Disqord;

/// <summary>
///     Represents a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) tied to a possibly unknown guild or application.
/// </summary>
public interface IOwnedEmoji : ICustomEmoji
{
    /// <summary>
    ///     Gets the user that created this emoji.
    /// </summary>
    IUser? Creator { get; }

    /// <summary>
    ///     Gets whether this emoji requires colons in chat.
    /// </summary>
    bool RequiresColons { get; }

    /// <summary>
    ///     Gets whether this emoji is managed by an integration.
    /// </summary>
    bool IsManaged { get; }
}