using System;

namespace Disqord.Gateway
{
    public class GuildEventCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the guild event was created.
        /// </summary>
        public Snowflake GuildId => Event.GuildId;

        /// <summary>
        ///     Gets the ID of the created guild event.
        /// </summary>
        public Snowflake EventId => Event.Id;

        /// <summary>
        ///     Gets the created guild event.
        /// </summary>
        public IGuildEvent Event { get; }

        public GuildEventCreatedEventArgs(IGuildEvent guildEvent)
        {
            Event = guildEvent;
        }
    }
}
