namespace Disqord;

/// <summary>
///     Represents the target of a guild event.
/// </summary>
public enum GuildEventTargetType
{
    /// <summary>
    ///     The guild event targets a stage channel.
    /// </summary>
    Stage = 1,

    /// <summary>
    ///     The guild event targets a voice channel.
    /// </summary>
    Voice = 2,

    /// <summary>
    ///     The guild event targets an external location.
    /// </summary>
    External = 3
}