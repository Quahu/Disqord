using System;

namespace Disqord.Gateway
{
    public class GuildEventUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the guild event was updated.
        /// </summary>
        public Snowflake GuildId => NewEvent.GuildId;

        /// <summary>
        ///     Gets the ID of the updated guild event.
        /// </summary>
        public Snowflake EventId => NewEvent.Id;

        /// <summary>
        ///     Gets the guild event in the state before the update occurred.
        ///     Returns <see langword="null"/> if the guild event was not cached.
        /// </summary>
        public IGuildEvent OldEvent { get; }

        /// <summary>
        ///     Gets the updated guild event.
        /// </summary>
        public IGuildEvent NewEvent { get; }

        public GuildEventUpdatedEventArgs(IGuildEvent oldGuildEvent, IGuildEvent newGuildEvent)
        {
            OldEvent = oldGuildEvent;
            NewEvent = newGuildEvent;
        }
    }
}
