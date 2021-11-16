using System;

namespace Disqord.Gateway
{
    public class GuildEventUserAddedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild which the guild event belongs to.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the guild event the user subscribed to.
        /// </summary>
        public Snowflake EventId { get; }

        /// <summary>
        ///     Gets the ID of the user who subscribed to the guild event.
        /// </summary>
        public Snowflake UserId { get; }

        public GuildEventUserAddedEventArgs(Snowflake guildId, Snowflake eventId, Snowflake userId)
        {
            GuildId = guildId;
            EventId = eventId;
            UserId = userId;
        }
    }
}
