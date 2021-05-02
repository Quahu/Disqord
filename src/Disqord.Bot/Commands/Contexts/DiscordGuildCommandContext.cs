using System;
using Disqord.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Bot
{
    public class DiscordGuildCommandContext : DiscordCommandContext
    {
        /// <summary>
        ///     Gets the ID of the guild the command is being executed in.
        /// </summary>
        public virtual new Snowflake GuildId => base.GuildId.Value;

        /// <summary>
        ///     Gets the guild the command is being executed in.
        /// </summary>
        public virtual CachedGuild Guild
        {
            get => _guild ??= Bot.GetGuild(GuildId);
            protected set => _guild = value;
        }
        private CachedGuild _guild;

        /// <summary>
        ///     Gets the bot member of the guild the command is being executed in.
        /// </summary>
        public virtual CachedMember CurrentMember
        {
            get => _currentMember ??= Bot.GetMember(GuildId, Bot.CurrentUser.Id);
            protected set => _currentMember = value;
        }
        private CachedMember _currentMember;
        
        /// <inheritdoc/>
        public virtual new IMember Author => base.Author as IMember;

        /// <summary>
        ///     Gets the channel the command is being executed in.
        /// </summary>
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
