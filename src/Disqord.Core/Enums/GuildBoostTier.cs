namespace Disqord;

/// <summary>
///     Represents the Nitro boost tier of a guild.
/// </summary>
public enum GuildBoostTier : byte
{
    /// <summary>
    ///     The guild has no boost tier.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The guild has the first boost tier.
    /// </summary>
    First = 1,

    /// <summary>
    ///     The guild has the second boost tier.
    /// </summary>
    Second = 2,

    /// <summary>
    ///     The guild has the third boost tier.
    /// </summary>
    Third = 3
}