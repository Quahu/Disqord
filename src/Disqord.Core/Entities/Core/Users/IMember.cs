using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a user in a guild.
/// </summary>
public interface IMember : IUser, IGuildEntity, IJsonUpdatable<MemberJsonModel>
{
    /// <summary>
    ///     Gets the nick of this member.
    /// </summary>
    /// <returns>
    ///     The nick or <see langword="null"/> if this member has no nick set.
    /// </returns>
    string? Nick { get; }

    /// <summary>
    ///     Gets the IDs of the roles this member has.
    /// </summary>
    IReadOnlyList<Snowflake> RoleIds { get; }

    /// <summary>
    ///     Gets when this member joined the guild.
    /// </summary>
    Optional<DateTimeOffset> JoinedAt { get; }

    /// <summary>
    ///     Gets whether this member is guild muted in voice channels.
    /// </summary>
    bool IsMuted { get; }

    /// <summary>
    ///     Gets whether this member is guild deafened in voice channels.
    /// </summary>
    bool IsDeafened { get; }

    /// <summary>
    ///     Gets when this member boosted the guild.
    /// </summary>
    /// <returns>
    ///     When the member boosted the guild or <see langword="null"/> if this member is not a Nitro booster.
    /// </returns>
    DateTimeOffset? BoostedAt { get; }

    /// <summary>
    ///     Gets whether this member has not completed the membership screening yet.
    /// </summary>
    bool IsPending { get; }

    /// <summary>
    ///     Gets the guild avatar image hash of this member.
    /// </summary>
    /// <returns>
    ///     The image hash of this member's guild avatar or <see langword="null"/> if this member has no guild avatar set.
    /// </returns>
    string? GuildAvatarHash { get; }

    /// <summary>
    ///     Gets until when this member is timed out.
    /// </summary>
    /// <returns>
    ///     When the member is timed out until or <see langword="null"/> if this member has not been timed out.
    /// </returns>
    DateTimeOffset? TimedOutUntil { get; }
}
