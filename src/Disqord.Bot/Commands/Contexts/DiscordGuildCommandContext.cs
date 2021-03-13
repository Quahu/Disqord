using System;
using Disqord.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Bot
{
    public class DiscordGuildCommandContext : DiscordCommandContext
    {
        public virtual new Snowflake GuildId => base.GuildId.Value;

        public virtual CachedGuild Guild
        {
            get
            {
                if (_guild == null)
                    _guild = Bot.GetGuild(GuildId);

                return _guild;
            }
            protected set => _guild = value;
        }
        private CachedGuild _guild;

        public virtual CachedMember CurrentMember
        {
            get
            {
                if (_currentMember == null)
                    _currentMember = Bot.GetMember(GuildId, Bot.CurrentUser.Id);

                return _currentMember;
            }
            protected set => _currentMember = value;
        }
        private CachedMember _currentMember;
        
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
