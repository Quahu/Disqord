namespace Disqord;

/// <summary>
///     Represents the type of a system message.
/// </summary>
/// <remarks>
///     This enumeration defines only the system message types. The remaining types are defined in <see cref="UserMessageType"/>.
/// </remarks>
public enum SystemMessageType
{
    /// <summary>
    ///     Represents a system message indicating a recipient has been added to a private channel.
    /// </summary>
    RecipientAdded = 0,

    /// <summary>
    ///     Represents a system message indicating a recipient has been removed from a private channel.
    /// </summary>
    RecipientRemoved = 1,

    /// <summary>
    ///     Represents a system message indicating a call has been started or missed.
    /// </summary>
    Call = 2,

    /// <summary>
    ///     Represents a system message indicating the name of a private channel has been changed.
    /// </summary>
    ChannelNameChanged = 3,

    /// <summary>
    ///     Represents a system message indicating the icon of a private channel has been changed.
    /// </summary>
    ChannelIconChanged = 4,

    /// <summary>
    ///     Represents a system message indicating a message a channel has been pinned.
    /// </summary>
    ChannelMessagePinned = 5,

    /// <summary>
    ///     Represents a system message indicating a new member has joined a guild.
    /// </summary>
    MemberJoined = 6,

    /// <summary>
    ///     Represents a system message indicating a member has boosted a guild.
    /// </summary>
    GuildBoosted = 7,

    /// <summary>
    ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="GuildBoostTier.First"/>.
    /// </summary>
    GuildBoostedFirstTier = 8,

    /// <summary>
    ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="GuildBoostTier.Second"/>.
    /// </summary>
    GuildBoostedSecondTier = 9,

    /// <summary>
    ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="GuildBoostTier.Third"/>.
    /// </summary>
    GuildBoostedThirdTier = 10,

    /// <summary>
    ///     Represents a system message indicating a member has followed another channel.
    /// </summary>
    ChannelFollowed = 11,

    /// <summary>
    ///     Represents a system message indicating a member is streaming.
    /// </summary>
    GuildStream = 12,

    /// <summary>
    ///     Represents a system message indicating the guild is no longer eligible for guild discovery.
    /// </summary>
    GuildDiscoveryDisqualified = 13,

    /// <summary>
    ///     Represents a system message indicating the guild is once again eligible for guild discovery.
    /// </summary>
    GuildDiscoveryRequalified = 14,

    /// <summary>
    ///     Represents a system message indicating the guild has not been eligible for guild discovery for a week.
    /// </summary>
    GuildDiscoveryGracePeriodInitialWarning = 15,

    /// <summary>
    ///     Represents a system message indicating the guild has not been eligible for guild discovery for 3 weeks.
    /// </summary>
    GuildDiscoveryGracePeriodFinalWarning = 16,

    /// <summary>
    ///     Represents a system message indicating a member has started a thread.
    /// </summary>
    ThreadCreated = 17,

    /// <summary>
    ///     Represents a system message indicating a reminder to invite members to a guild.
    /// </summary>
    GuildInviteReminder = 21
}