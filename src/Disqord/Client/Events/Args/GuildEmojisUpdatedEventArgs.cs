using System.Collections.Generic;

namespace Disqord.Events
{
    public sealed class GuildEmojisUpdatedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public IReadOnlyList<CachedGuildEmoji> OldEmojis { get; }

        public IReadOnlyList<CachedGuildEmoji> NewEmojis { get; }

        internal GuildEmojisUpdatedEventArgs(CachedGuild guild, IReadOnlyList<CachedGuildEmoji> oldEmojis) : base(guild.Client)
        {
            Guild = guild;
            OldEmojis = oldEmojis;
            NewEmojis = guild.Emojis;
        }
    }
}
