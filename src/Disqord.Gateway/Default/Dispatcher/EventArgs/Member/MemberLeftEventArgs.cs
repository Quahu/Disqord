using System;

namespace Disqord.Gateway
{
    public class MemberLeftEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild from which the user left.
        /// </summary>
        public Snowflake GuildId { get; }
        
        /// <summary>
        ///     Gets the user that left.
        /// </summary>
        public IUser User { get; }

        public MemberLeftEventArgs(Snowflake guildId, IUser user)
        {
            GuildId = guildId;
            User = user;
        }
    }
}
