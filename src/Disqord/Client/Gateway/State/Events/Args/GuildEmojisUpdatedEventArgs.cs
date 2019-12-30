using System.Collections.Generic;

namespace Disqord.Events
{
    public sealed class GuildEmojisUpdatedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public IReadOnlyDictionary<Snowflake, CachedGuildEmoji> OldEmojis { get; }

        public IReadOnlyDictionary<Snowflake, CachedGuildEmoji> NewEmojis { get; }

        internal GuildEmojisUpdatedEventArgs(CachedGuild guild, IReadOnlyDictionary<Snowflake, CachedGuildEmoji> oldEmojis) : base(guild.Client)
        {
            Guild = guild;
            OldEmojis = oldEmojis;
            NewEmojis = guild.Emojis;
        }
    }
}
