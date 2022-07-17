namespace Disqord;

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
    /// </summary>
    /// <returns>
    ///     The target type or <see langword="null"/> when this invite's channel is not a voice channel.
    /// </returns>
    InviteTargetType? TargetType { get; }

    /// <summary>
    ///     Gets the optional target user of this invite.
    /// </summary>
    /// <returns>
    ///     The target user or <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.Stream"/>.
    /// </returns>
    IUser? TargetUser { get; }

    /// <summary>
    ///     Gets the optional target application of this invite.
    /// </summary>
    /// <returns>
    ///     The target application or <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.EmbeddedApplication"/>.
    /// </returns>
    IApplication? TargetApplication { get; }

    /// <summary>
    ///     Gets the optional stage of this invite.
    /// </summary>
    /// <returns>
    ///     The stage or <see langword="null"/> when this invite's channel is not a stage channel.
    /// </returns>
    IInviteStage? Stage { get; }

    /// <summary>
    ///     Gets the optional guild event tied to this invite.
    /// </summary>
    /// <remarks>
    ///     Returned when the invite is fetched by code with the <c>eventId</c> parameter set to a valid guild event ID.
    /// </remarks>
    /// <returns>
    ///     The event or <see langword="null"/> when this invite's guild event ID parameter is not a valid guild event ID.
    /// </returns>
    IGuildEvent? Event { get; }

    /// <summary>
    ///     Gets the approximate presence count of the guild of this invite.
    /// </summary>
    /// <remarks>
    ///     Returned when the invite is fetched by code with the <c>withCounts</c> parameter set to <see langword="true" />.
    /// </remarks>
    /// <returns>
    ///     The approximate presence count or <see langword="null"/> when this invite's channel is a group channel.
    /// </returns>
    int? ApproximatePresenceCount { get; }

    /// <summary>
    ///     Gets the optional metadata of this invite.
    /// </summary>
    IInviteMetadata? Metadata { get; }
}
