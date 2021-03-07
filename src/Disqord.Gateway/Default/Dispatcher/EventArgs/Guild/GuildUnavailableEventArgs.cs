using System;

namespace Disqord.Gateway
{
    public class GuildUnavailableEventArgs : EventArgs
    {
        public Snowflake GuildId { get; }

        public CachedGuild Guild { get; }

        public GuildUnavailableEventArgs(Snowflake guildId, CachedGuild guild)
        {
            GuildId = guildId;
            Guild = guild;
        }
    }
}
