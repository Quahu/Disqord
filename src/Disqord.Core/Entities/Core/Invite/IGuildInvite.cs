namespace Disqord
{
    /// <summary>
    ///     Represents a guild invite.
    /// </summary>
    public interface IGuildInvite : IInvite, IGuildEntity
    {
        /// <summary>
        ///     Gets the guild this invite was created for.
        /// </summary>
        IInviteGuild Guild { get; }

        /// <summary>
        ///     Gets the optional target type of this invite.
        ///     Returns <see langword="null"/> when this invite's channel is not a voice channel.
        /// </summary>
        InviteTargetType? TargetType { get; }

        /// <summary>
        ///     Gets the optional target user of this invite.
        ///     Returns <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.Stream"/>.
        /// </summary>
        IUser TargetUser { get; }

        /// <summary>
        ///     Gets the optional target application of this invite.
        ///     Returns <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.EmbeddedApplication"/>.
        /// </summary>
        IApplication TargetApplication { get; }

        /// <summary>
        ///     Gets the optional stage of this invite.
        ///     Returns <see langword="null"/> when this invite's channel is not a stage channel.
        /// </summary>
        IInviteStage Stage { get; }

        /// <summary>
        ///     Gets the optional guild event tied to this invite.
        ///     Returns <see langword="null"/> when this invite's guild event ID parameter is not a valid guild event ID.
        /// </summary>
        /// <remarks>
        ///     Returned when the invite is fetched by code with the <c>eventId</c> parameter set to a valid guild event ID.
        /// </remarks>
        IGuildEvent Event { get; }

        /// <summary>
        ///     Gets the approximate presence count of the guild of this invite.
        ///     Returns <see langword="null"/> when this invite's channel is a group channel.
        /// </summary>
        /// <remarks>
        ///     Returned when the invite is fetched by code with the <c>withCounts</c> parameter set to <see langword="true" />.
        /// </remarks>
        int? ApproximatePresenceCount { get; }

        /// <summary>
        ///     Gets the optional metadata of this invite.
        /// </summary>
        IInviteMetadata Metadata { get; }
    }
}
