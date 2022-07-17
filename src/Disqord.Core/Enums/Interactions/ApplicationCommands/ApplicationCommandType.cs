namespace Disqord;

/// <summary>
///     Represents the type of an application command.
/// </summary>
public enum ApplicationCommandType : byte
{
    /// <summary>
    ///     A text based command that shows up when a user types <c>&#47;</c>.
    /// </summary>
    Slash = 1,

    /// <summary>
    ///     A UI based command that shows up in a user context menu.
    /// </summary>
    User = 2,

    /// <summary>
    ///     A UI based command that shows up in a message context menu.
    /// </summary>
    Message = 3
}