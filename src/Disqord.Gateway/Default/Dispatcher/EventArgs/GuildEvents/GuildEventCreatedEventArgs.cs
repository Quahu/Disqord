using System;

namespace Disqord.Gateway
{
    public class GuildEventCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the event was created.
        /// </summary>
        public Snowflake GuildId => Event.GuildId;

        /// <summary>
        ///     Gets the ID of the created event.
        /// </summary>
        public Snowflake EventId => Event.Id;

        /// <summary>
        ///     Gets the created event.
        /// </summary>
        public IGuildEvent Event { get; }

        public GuildEventCreatedEventArgs(IGuildEvent @event)
        {
            Event = @event;
        }
    }
}
