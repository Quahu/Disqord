using System;

namespace Disqord.Gateway
{
    public class GuildEventUserRemovedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild which the guild event belongs to.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the ID of the guild event the user unsubscribed from.
        /// </summary>
        public Snowflake EventId { get; }

        /// <summary>
        ///     Gets the ID of the user who unsubscribed from the guild event.
        /// </summary>
        public Snowflake UserId { get; }

        public GuildEventUserRemovedEventArgs(Snowflake guildId, Snowflake eventId, Snowflake userId)
        {
            GuildId = guildId;
            EventId = eventId;
            UserId = userId;
        }
    }
}
