using System;

namespace Disqord.Gateway
{
    public class BanDeletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the guild ID the ban was created in.
        /// </summary>
        public Snowflake GuildId { get; }

        /// <summary>
        ///     Gets the user the was created for.
        /// </summary>
        public IUser User { get; }

        public BanDeletedEventArgs(Snowflake guildId, IUser user)
        {
            GuildId = guildId;
            User = user;
        }
    }
}
