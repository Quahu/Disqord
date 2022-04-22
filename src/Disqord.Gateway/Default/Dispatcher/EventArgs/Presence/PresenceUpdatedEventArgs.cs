using System;
using Qommon;

namespace Disqord.Gateway
{
    public class PresenceUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild the presence was updated in.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the member the presence was updated for.
        /// </summary>
        public Snowflake MemberId => NewPresence.MemberId;

        /// <summary>
        ///     Gets the optional user object the presence contained.
        /// </summary>
        /// <remarks>
        ///     This is not a reliable property. It is not provided for most updates.
        ///     The member should be instead retrieved from the client's cache or via REST.
        /// </remarks>
        public Optional<IUser> User { get; }

        /// <summary>
        ///     Gets the presence in the state before the update occurred.
        ///     Returns <see langword="null"/> if the presence was not cached.
        /// </summary>
        public CachedPresence OldPresence { get; }

        /// <summary>
        ///     Gets the updated presence.
        /// </summary>
        public IPresence NewPresence { get; }

        public PresenceUpdatedEventArgs(
            Snowflake guildId,
            Optional<IUser> user,
            CachedPresence oldPresence,
            IPresence newPresence)
        {
            GuildId = guildId;
            User = user;
            OldPresence = oldPresence;
            NewPresence = newPresence;
        }
    }
}
