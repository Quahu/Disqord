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
            IPrefix prefix,
            IGatewayUserMessage message,
            ITextChannel channel,
            IServiceProvider services)
            : base(bot, prefix, message, services)
        {
            Channel = channel;
        }
    }
}
