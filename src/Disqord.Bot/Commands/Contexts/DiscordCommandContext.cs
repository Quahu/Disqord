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
        /// <summary>
        ///     Gets the bot client.
        /// </summary>
        public virtual DiscordBotBase Bot { get; }

        /// <summary>
        ///     Gets the prefix that was used for the command invocation.
        /// </summary>
        public virtual IPrefix Prefix { get; }

        /// <summary>
        ///     Gets the input without the prefix provided for command invocation.
        /// </summary>
        public virtual string Input { get; }

        /// <summary>
        ///     Gets the ID of the guild the command is being executed in.
        ///     Returns <see langword="null"/> if the command is being executed in a private channel.
        /// </summary>
        public virtual Snowflake? GuildId => Message.GuildId;

        /// <summary>
        ///     Gets the source message that invoked the command.
        /// </summary>
        public virtual IGatewayUserMessage Message { get; }

        /// <summary>
        ///     Gets the ID of the channel the command is being executed in.
        /// </summary>
        public virtual Snowflake ChannelId => Message.ChannelId;

        /// <summary>
        ///     Gets the author who invoked the command.
        /// </summary>
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
            string input,
            IGatewayUserMessage message,
            IServiceProvider services)
            : base(services)
        {
            Bot = bot;
            Prefix = prefix;
            Input = input;
            Message = message;
        }

        public DiscordCommandContext(
            DiscordBotBase bot,
            IPrefix prefix,
            string input,
            IGatewayUserMessage message,
            IServiceScope serviceScope)
            : this(bot, prefix, input, message, serviceScope.ServiceProvider)
        {
            _serviceScope = serviceScope;
        }

        /// <summary>
        ///     Yields this command execution flow into the background
        ///     freeing up a slot in the <see cref="ICommandQueue"/>.
        /// </summary>
        /// <example>
        ///     Yielding the command execution flow before cheap background work.
        ///     See <see cref="ContinueAsync"/> for how to re-enter the <see cref="ICommandQueue"/>.
        ///     <code>
        ///     Context.Yield();
        ///     await Task.Delay(5000); // Command code after Yield() executes in the background.
        ///     </code>
        /// </example>
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
        /// <example>
        ///     Re-entering the <see cref="ICommandQueue"/> after <see cref="Yield"/>ing.
        ///     <code>
        ///     await Context.ContinueAsync();
        ///     // Command code executes in the queue again.
        ///     </code>
        /// </example>
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
            Bot.Queue.Post(this, null);
            await continuationTcs.Task.ConfigureAwait(false);
        }

        /// <summary>
        ///     Calls <see cref="Yield"/> and returns a disposable that
        ///     calls <see cref="ContinueAsync"/> when disposed.
        ///     Handy for grouping up background logic.
        /// </summary>
        /// <example>
        ///     Yielding the command execution flow and then re-entering automatically.
        ///     See <see cref="Yield"/> and <see cref="ContinueAsync"/>.
        ///     <code>
        ///     await using (Context.BeginYield())
        ///     {
        ///         // Code to execute in the background.
        ///     }
        ///     </code>
        /// </example>
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
        ///     This method is automatically called by <see cref="DiscordBotBase"/>.
        /// </summary>
        /// <returns>
        ///     A <see cref="ValueTask"/> representing the disposal work.
        /// </returns>
        public virtual ValueTask DisposeAsync()
            => RuntimeDisposal.DisposeAsync(_serviceScope);
    }
}
