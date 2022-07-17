namespace Disqord;

/// <summary>
///     Represents the type of a user message.
/// </summary>
/// <remarks>
///     This enumeration defines only the non-system types. The remaining types are defined in <see cref="SystemMessageType"/>.
/// </remarks>
public enum UserMessageType
{
    /// <summary>
    ///     Represents a normal message sent by a user.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     Represents a message that is a reply to another message.
    /// </summary>
    Reply = 19,

    /// <summary>
    ///     Represents a slash command message.
    /// </summary>
    SlashCommand = 20,

    /// <summary>
    ///     Represents the first message in a thread, i.e. the message that started it.
    /// </summary>
    ThreadStarterMessage = 21,

    /// <summary>
    ///     Represents a context menu command message.
    /// </summary>
    ContextMenuCommand = 23,

    /// <summary>
    ///     Represents an auto-moderation action alert message.
    /// </summary>
    AutoModerationAction = 24
}