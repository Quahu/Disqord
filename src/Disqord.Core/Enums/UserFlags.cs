using System;

namespace Disqord;

/// <summary>
///     Represents the flags of a user.
/// </summary>
[Flags]
public enum UserFlags : uint
{
    /// <summary>
    ///     The user has no flags.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The user is a Discord staff member.
    /// </summary>
    Staff = 1 << 0,

    /// <summary>
    ///     The user is a Discord partner.
    /// </summary>
    Partner = 1 << 1,

    /// <summary>
    ///     The user organizes HypeSquad events.
    /// </summary>
    HypeSquad = 1 << 2,

    /// <summary>
    ///     The user is a first level bug hunter.
    /// </summary>
    FirstLevelBugHunter = 1 << 3,

    /// <summary>
    ///     The user is a part of the HypeSquad Bravery house.
    /// </summary>
    HypeSquadBravery = 1 << 6,

    /// <summary>
    ///     The user is a part of the HypeSquad Brilliance house.
    /// </summary>
    HypeSquadBrilliance = 1 << 7,

    /// <summary>
    ///     The user is a part of the HypeSquad Balance house.
    /// </summary>
    HypeSquadBalance = 1 << 8,

    /// <summary>
    ///     The user is an early Nitro supporter.
    /// </summary>
    EarlySupporter = 1 << 9,

    /// <summary>
    ///     The user is a pseudo user object returned for application teams.
    /// </summary>
    TeamUser = 1 << 10,

    /// <summary>
    ///     The user is a second level bug hunter.
    /// </summary>
    SecondLevelBugHunter = 1 << 14,

    /// <summary>
    ///     The user is a verified bot.
    /// </summary>
    VerifiedBot = 1 << 16,

    /// <summary>
    ///     The user is an early verified bot developer.
    /// </summary>
    EarlyVerifiedBotDeveloper = 1 << 17,

    /// <summary>
    ///     The user is a Discord certified moderator.
    /// </summary>
    CertifiedModerator = 1 << 18,

    /// <summary>
    ///     The user is a bot using HTTP interactions and is shown online in member lists.
    /// </summary>
    HttpInteractionsBot = 1 << 19
}
