using System;

namespace Disqord.Gateway
{
    public class BanCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the guild ID the ban was created in.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the user the was created for.
        /// </summary>
        public IUser User { get; }

        public BanCreatedEventArgs(Snowflake guildId, IUser user)
        {
            GuildId = guildId;
            User = user;
        }
    }
}
