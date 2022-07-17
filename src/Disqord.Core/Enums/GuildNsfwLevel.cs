namespace Disqord;

/// <summary>
///     Represents the NSFW level of a guild.
/// </summary>
public enum GuildNsfwLevel : byte
{
    /// <summary>
    ///     The guild uses default NSFW settings.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     The guild contains explicit NSFW content.
    /// </summary>
    Explicit = 1,

    /// <summary>
    ///     The guild does not contain NSFW content.
    /// </summary>
    Safe = 2,

    /// <summary>
    ///     The guild contains age restricted content.
    /// </summary>
    AgeRestricted = 3
}