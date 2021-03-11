using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents a Discord bot command context.
    /// </summary>
    public class DiscordCommandContext : CommandContext, IAsyncDisposable
    {
        public virtual DiscordBotBase Bot { get; }

        public virtual IPrefix Prefix { get; }

        public virtual Snowflake? GuildId => Message.GuildId;

        public virtual IGatewayUserMessage Message { get; }

        public virtual Snowflake ChannelId => Message.ChannelId;

        public virtual IUser Author => Message.Author;

        internal Tcs YieldTcs;

        private readonly IServiceScope _serviceScope;

        public DiscordCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            IGatewayUserMessage message,
            IServiceProvider services)
            : base(services)
        {
            Bot = bot;
            Prefix = prefix;
            Message = message;

            YieldTcs = new Tcs();
        }

        public DiscordCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            IGatewayUserMessage message,
            IServiceScope serviceScope)
            : this(bot, prefix, message, serviceScope.ServiceProvider)
        {
            _serviceScope = serviceScope;
        }

        /// <summary>
        ///     Yields this command execution flow into the background.
        /// </summary>
        public void Yield()
        {
            YieldTcs.Complete();
        }

        public virtual ValueTask DisposeAsync()
            => RuntimeDisposal.DisposeAsync(_serviceScope);
    }
}
