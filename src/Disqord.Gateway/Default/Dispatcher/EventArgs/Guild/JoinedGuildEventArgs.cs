using System;

namespace Disqord.Gateway
{
    public class JoinedGuildEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild the current user joined.
        /// </summary>
        public Snowflake GuildId => Guild.Id;

        /// <summary>
        ///     Gets the guild the current user joined.
        /// </summary>
        public IGatewayGuild Guild { get; }

        public JoinedGuildEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}