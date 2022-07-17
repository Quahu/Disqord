namespace Disqord;

/// <summary>
///     Represents the privacy level of a Discord stage or guild event.
/// </summary>
public enum PrivacyLevel
{
    /// <summary>
    ///     The stage or event is visible publicly.
    /// </summary>
    Public = 1,

    /// <summary>
    ///     The stage or event is visible to members only.
    /// </summary>
    GuildOnly = 2
}
