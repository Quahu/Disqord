using System;
using Disqord.Gateway;

namespace Disqord.Bot
{
    public class DiscordGuildCommandContext : DiscordCommandContext
    {
        public virtual new Snowflake GuildId => base.GuildId.Value;

        public virtual new IMember Author => base.Author as IMember;

        public virtual ITextChannel Channel { get; }

        public DiscordGuildCommandContext(
            DiscordBotBase bot,
            IGatewayUserMessage message,
            ITextChannel channel,
            IServiceProvider services)
            : base(bot, message, services)
        {
            Channel = channel;
        }
    }
}
