using System;

namespace Disqord.Gateway
{
    public class GuildAvailableEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild that became available.
        /// </summary>
        public Snowflake GuildId => Guild.Id;

        /// <summary>
        ///     Gets the guild that became available.
        /// </summary>
        public IGatewayGuild Guild { get; }

        public GuildAvailableEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}
