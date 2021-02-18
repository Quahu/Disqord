using System;
using Qmmands;

namespace Disqord.Bot
{
    public class DiscordCommandContext : CommandContext
    {
        public virtual DiscordBotBase Bot { get; }

        public virtual IUserMessage Message { get; }

        public virtual IMessageChannel Channel { get; }

        public virtual IUser User => Message.Author;

        public virtual IMember Member => User as IMember;

        public DiscordCommandContext(
            DiscordBotBase bot,
            IUserMessage message,
            IServiceProvider services)
            : base(services)
        {
            Bot = bot;
            Message = message;
        }
    }
}
