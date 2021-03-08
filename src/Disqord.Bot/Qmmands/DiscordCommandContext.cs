using System;
using Disqord.Gateway;
using Disqord.Utilities.Threading;
using Qmmands;

namespace Disqord.Bot
{
    public class DiscordCommandContext : CommandContext
    {
        public virtual DiscordBotBase Bot { get; }

        public virtual IPrefix Prefix { get; }

        public virtual Snowflake? GuildId => Message.GuildId;

        public virtual IGatewayUserMessage Message { get; }

        public virtual Snowflake ChannelId => Message.ChannelId;

        public virtual IUser Author => Message.Author;

        internal Tcs YieldTcs;

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

        /// <summary>
        ///     Yields this command execution flow into the background.
        /// </summary>
        public void Yield()
        {
            YieldTcs.Complete();
        }
    }
}
