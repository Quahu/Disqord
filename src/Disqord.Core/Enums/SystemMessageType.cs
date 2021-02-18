namespace Disqord
{
    /// <summary>
    ///     Represents the type of a message.
    /// </summary>
    public enum SystemMessageType
    {
        /// <summary>
        ///     Represents a system message indicating a recipient has been added to a private channel.
        /// </summary>
        RecipientAdded,

        /// <summary>
        ///     Represents a system message indicating a recipient has been removed from a private channel.
        /// </summary>
        RecipientRemoved,

        /// <summary>
        ///     Represents a system message indicating a call has been started or missed.
        /// </summary>
        Call,

        /// <summary>
        ///     Represents a system message indicating the name of a private channel has been changed.
        /// </summary>
        ChannelNameChanged,

        /// <summary>
        ///     Represents a system message indicating the icon of a private channel has been changed.
        /// </summary>
        ChannelIconChanged,

        /// <summary>
        ///     Represents a system message indicating a message a channel has been pinned.
        /// </summary>
        ChannelMessagePinned,

        /// <summary>
        ///     Represents a system message indicating a new member has joined a guild.
        /// </summary>
        MemberJoined,

        /// <summary>
        ///     Represents a system message indicating a member has boosted a guild.
        /// </summary>
        GuildBoosted,

        /// <summary>
        ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="BoostTier.First"/>.
        /// </summary>
        GuildBoostedFirstTier,

        /// <summary>
        ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="BoostTier.Second"/>.
        /// </summary>
        GuildBoostedSecondTier,

        /// <summary>
        ///     Represents a system message indicating a member has boosted a guild and it achieved <see cref="BoostTier.Third"/>.
        /// </summary>
        GuildBoostedThirdTier,

        /// <summary>
        ///     Represents a system message indicating a member has followed another channel.
        /// </summary>
        ChannelFollowed,

        /// <summary>
        ///     Represents a system message indicating a member is streaming.
        /// </summary>
        GuildStream,

        /// <summary>
        ///     Represents a system message indicating the guild is no longer eligible for guild discovery.
        /// </summary>
        GuildDiscoveryDisqualified,

        /// <summary>
        ///     Represents a system message indicating the guild is once again eligible for guild discovery. 
        /// </summary>
        GuildDiscoveryRequalified,

        /// <summary>
        ///     Represents a system message indicating the guild has not been eligible for guild discovery for a week.
        /// </summary>
        GuildDiscoveryGracePeriodInitialWarning,

        /// <summary>
        ///     Represents a system message indicating the guild has not been eligible for guild discovery for 3 weeks.
        /// </summary>
        GuildDiscoveryGracePeriodFinalWarning,

        /// <summary>
        ///     Represents a system message indicating a member has started a thread.
        /// </summary>
        ThreadCreated
    }
}
