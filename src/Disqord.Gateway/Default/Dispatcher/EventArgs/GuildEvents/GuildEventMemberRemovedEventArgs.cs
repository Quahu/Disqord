using System;

namespace Disqord.Gateway
{
    public class GuildEventMemberRemovedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild which the event belongs to.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the event the user unsubscribed from.
        /// </summary>
        public Snowflake EventId { get; }

        /// <summary>
        ///     Gets the ID of the user who unsubscribed from the event.
        /// </summary>
        public Snowflake UserId { get; }

        public GuildEventMemberRemovedEventArgs(Snowflake guildId, Snowflake eventId, Snowflake userId)
        {
            GuildId = guildId;
            EventId = eventId;
            UserId = userId;
        }
    }
}
