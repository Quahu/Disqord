using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user in a guild.
    /// </summary>
    public interface IMember : IUser, IGuildEntity, IJsonUpdatable<MemberJsonModel>
    {
        /// <summary>
        ///     Gets the nick of this member.
        /// </summary>
        string Nick { get; }

        /// <summary>
        ///     Gets the role IDs of this member.
        /// </summary>
        IReadOnlyList<Snowflake> RoleIds { get; }

        /// <summary>
        ///     Gets when this member joined the guild.
        /// </summary>
        Optional<DateTimeOffset> JoinedAt { get; }

        /// <summary>
        ///     Gets whether this member is muted.
        /// </summary>
        bool IsMuted { get; }

        /// <summary>
        ///     Gets whether this member is deafened.
        /// </summary>
        bool IsDeafened { get; }

        /// <summary>
        ///     Gets when this member boosted the guild.
        /// </summary>
        DateTimeOffset? BoostedAt { get; }

        /// <summary>
        ///     Gets whether this member has not completed the membership screening yet.
        /// </summary>
        bool IsPending { get; }
    }
}
