using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents a user in a guild.
    /// </summary>
    public interface IMember : IUser, IGuildEntity, IJsonUpdatable<MemberJsonModel>
    {
        /// <summary>
        ///     Gets the nick of this member.
        ///     Returns <see langword="null"/> if this member has no nick set.
        /// </summary>
        string Nick { get; }

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
        ///     Returns <see langword="null"/> if this member is not a Nitro booster.
        /// </summary>
        DateTimeOffset? BoostedAt { get; }

        /// <summary>
        ///     Gets whether this member has not completed the membership screening yet.
        /// </summary>
        bool IsPending { get; }

        /// <summary>
        ///     Gets the guild avatar image hash of this member.
        ///     Returns <see langword="null"/> if this member has no guild avatar set.
        /// </summary>
        string GuildAvatarHash { get; }

        /// <summary>
        ///     Gets until when this member is timed out.
        ///     Returns <see langword="null"/> if this member has not been timed out.
        /// </summary>
        DateTimeOffset? TimedOutUntil { get; }
    }
}
