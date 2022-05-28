namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents the state of an application command.
/// </summary>
public enum ApplicationCommandChange : byte
{
    /// <summary>
    ///     The command was unchanged.
    /// </summary>
    None,

    /// <summary>
    ///     The command was created.
    /// </summary>
    Created,

    /// <summary>
    ///     The command was modified.
    /// </summary>
    Modified
}
