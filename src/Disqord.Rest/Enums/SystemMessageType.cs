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
        ChannelFollowed
    }
}
