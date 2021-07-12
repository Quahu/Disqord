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
        public override IMember Author => base.Author as IMember;

        /// <summary>
        ///     Gets the channel the command is being executed in.
        /// </summary>
        public virtual CachedMessageGuildChannel Channel { get; }

        public DiscordGuildCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            string input,
            IGatewayUserMessage message,
            CachedMessageGuildChannel channel,
            IServiceProvider services)
            : base(bot, prefix, input, message, services)
        {
            Channel = channel;
        }

        public DiscordGuildCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            string input,
            IGatewayUserMessage message,
            CachedMessageGuildChannel channel,
            IServiceScope serviceScope)
            : base(bot, prefix, input, message, serviceScope)
        {
            Channel = channel;
        }
    }
}
