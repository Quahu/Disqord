using System;
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

        /// <summary>
        ///     Instantiates a new <see cref="DiscordCommandContext"/>.
        /// </summary>
        /// <param name="bot"> The bot instance. </param>
        /// <param name="prefix"> The prefix found in the source message. </param>
        /// <param name="message"> The source message. </param>
        /// <param name="provider"> The optional <see cref="IServiceProvider"/>. If <see langword="null"/>, defaults to <paramref name="bot"/>. </param>
        public DiscordCommandContext(DiscordBotBase bot, IPrefix prefix, CachedUserMessage message, IServiceProvider provider = null) : base(provider ?? bot)
        {
            Bot = bot;
            Prefix = prefix;
            Message = message;
        }
    }
}
