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

        // Reset by Kamaji on continuations, completed by Yield().
        internal Tcs YieldTcs = new();

        // (Re)set by Yield(), completed by Kamaji.
        internal Tcs ContinuationTcs;

        // Set by Kamaji on Post().
        internal Task Task;

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
        ///     Yields this command execution flow into the background
        ///     freeing up a spot in the <see cref="ICommandQueue"/>.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the flow yielded.
        /// </returns>
        public bool Yield()
        {
            if (YieldTcs.Complete())
            {
                ContinuationTcs = new Tcs();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Asynchronously waits for this command execution flow to
        ///     be executed in the <see cref="ICommandQueue"/> again.
        /// </summary>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the wait.
        /// </returns>
        public async ValueTask ContinueAsync()
        {
            var continuationTcs = ContinuationTcs;
            if (!YieldTcs.Task.IsCompleted || continuationTcs == null || continuationTcs.Task.IsCompleted)
                return;

            // Kamaji will treat the bath token as existing work
            // and complete the continuation TCS when there's a spot in the queue.
            Bot.Queue.Post(null, this, null);
            await continuationTcs.Task.ConfigureAwait(false);
        }

        /// <summary>
        ///     Calls <see cref="Yield"/> and returns a disposable that
        ///     calls <see cref="ContinueAsync"/> when disposed.
        /// </summary>
        /// <returns>
        ///     A disposable which calls <see cref="ContinueAsync"/> when disposed.
        /// </returns>
        public YieldDisposable BeginYield()
        {
            Yield();
            return new YieldDisposable(this);
        }

        public readonly struct YieldDisposable : IAsyncDisposable
        {
            private readonly DiscordCommandContext _context;

            public YieldDisposable(DiscordCommandContext context)
            {
                _context = context;
            }

            public ValueTask DisposeAsync()
                => _context.ContinueAsync();
        }

        /// <summary>
        ///     Disposes this <see cref="DiscordCommandContext"/> and its underlying <see cref="IServiceScope"/>.
        /// </summary>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the disposal work.
        /// </returns>
        public virtual ValueTask DisposeAsync()
            => RuntimeDisposal.DisposeAsync(_serviceScope);
    }
}
