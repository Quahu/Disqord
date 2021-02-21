using System;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class DiscordCommandContext : CommandContext
    {
        public virtual DiscordBotBase Bot { get; }

        public virtual Snowflake? GuildId => Message.GuildId;

        public virtual IGatewayUserMessage Message { get; }

        public virtual Snowflake ChannelId => Message.ChannelId;

        public virtual IUser Author => Message.Author;

        public DiscordCommandContext(
            DiscordBotBase bot,
            IGatewayUserMessage message,
            IServiceProvider services)
            : base(services)
        {
            Bot = bot;
            Message = message;
        }
    }
}
