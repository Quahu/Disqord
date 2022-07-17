using System;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a channel invite.
/// </summary>
public interface IInvite : IPossiblyChannelEntity, IClientEntity, IJsonUpdatable<InviteJsonModel>
{
    /// <summary>
    ///     Gets the code of this invite.
    /// </summary>
    string Code { get; }

    /// <summary>
    ///     Gets the channel this invite was created for.
    /// </summary>
    /// <returns>
    ///     The channel or <see langword="null"/> if the channel was not provided with this invite
    ///     like with, for example, friend invites.
    /// </returns>
    IInviteChannel? Channel { get; }

    /// <summary>
    ///     Gets the optional user who created this invite.
    /// </summary>
    IUser? Inviter { get; }

    /// <summary>
    ///     Gets the approximate member count of the guild of this invite.
    /// </summary>
    /// <remarks>
    ///     Returned when the invite is fetched by code with the <c>withCounts</c> parameter set to <see langword="true"/>.
    /// </remarks>
    int? ApproximateMemberCount { get; }

    /// <summary>
    ///     Gets when this invite expires.
    /// </summary>
    /// <remarks>
    ///     Returned when the invite is fetched by code with the <c>withExpiration</c> parameter set to <see langword="true"/>.
    /// </remarks>
    /// <returns>
    ///     When this invite expires at or <see langword="null"/> when this invite has no expiration.
    /// </returns>
    DateTimeOffset? ExpiresAt { get; }
}
