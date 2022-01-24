using System;

namespace Disqord.Gateway
{
    public class GuildEventMemberAddedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild which the event belongs to.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the event the user subscribed to.
        /// </summary>
        public Snowflake EventId { get; }

        /// <summary>
        ///     Gets the ID of the user who subscribed to the event.
        /// </summary>
        public Snowflake UserId { get; }

        public GuildEventMemberAddedEventArgs(Snowflake guildId, Snowflake eventId, Snowflake userId)
        {
            GuildId = guildId;
            EventId = eventId;
            UserId = userId;
        }
    }
}
