using System;
using Disqord.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Bot
{
    public class DiscordGuildCommandContext : DiscordCommandContext
    {
        public virtual new Snowflake GuildId => base.GuildId.Value;

        public virtual new IMember Author => base.Author as IMember;

        public virtual CachedTextChannel Channel { get; }

        public DiscordGuildCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            IGatewayUserMessage message,
            CachedTextChannel channel,
            IServiceProvider services)
            : base(bot, prefix, message, services)
        {
            Channel = channel;
        }

        public DiscordGuildCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            IGatewayUserMessage message,
            CachedTextChannel channel,
            IServiceScope serviceScope)
            : base(bot, prefix, message, serviceScope)
        {
            Channel = channel;
        }
    }
}
