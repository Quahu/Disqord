using System;

namespace Disqord.Gateway
{
    public class GuildEventUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the event was updated.
        /// </summary>
        public Snowflake GuildId => NewEvent.GuildId;

        /// <summary>
        ///     Gets the ID of the updated event.
        /// </summary>
        public Snowflake EventId => NewEvent.Id;

        /// <summary>
        ///     Gets the event in the state before the update occurred.
        ///     Returns <see langword="null"/> if the event was not cached.
        /// </summary>
        public CachedGuildEvent OldEvent { get; }

        /// <summary>
        ///     Gets the updated guild event.
        /// </summary>
        public IGuildEvent NewEvent { get; }

        public GuildEventUpdatedEventArgs(CachedGuildEvent oldGuildEvent, IGuildEvent newGuildEvent)
        {
            OldEvent = oldGuildEvent;
            NewEvent = newGuildEvent;
        }
    }
}
