using Disqord.Bot.Prefixes;
using Qmmands;

namespace Disqord.Bot
{
    public class DiscordCommandContext : CommandContext
    {
        public virtual DiscordBotBase Bot { get; }

        public virtual IPrefix Prefix { get; }

        public virtual CachedUserMessage Message { get; }

        public virtual ICachedMessageChannel Channel => Message.Channel;

        public virtual CachedUser User => Message.Author;

        public virtual CachedMember Member => User as CachedMember;

        public virtual CachedGuild Guild => Member?.Guild;

        public DiscordCommandContext(DiscordBotBase bot, IPrefix prefix, CachedUserMessage message) : base(bot)
        {
            Bot = bot;
            Prefix = prefix;
            Message = message;
        }
    }
}
