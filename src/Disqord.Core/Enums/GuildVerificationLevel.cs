namespace Disqord;

/// <summary>
///     Represents the verification level of a guild.
/// </summary>
public enum GuildVerificationLevel : byte
{
    /// <summary>
    ///     The guild does not have a verification level.
    /// </summary>
    None,

    /// <summary>
    ///     Members must have a verified email on their account.
    /// </summary>
    Low,

    /// <summary>
    ///     Members must also be registered on Discord for longer than 5 minutes.
    /// </summary>
    Medium,

    /// <summary>
    ///     Members must be members of the guild for longer than 10 minutes.
    /// </summary>
    High,

    /// <summary>
    ///     Members must have a verified phone on their Discord account.
    /// </summary>
    VeryHigh
}
