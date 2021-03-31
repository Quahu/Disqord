using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway;
using Disqord.Utilities.Binding;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Options;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents the default implementation of an <see cref="ICommandQueue"/>.
    ///     Handles command execution based on the set degree of parallelism.
    /// </summary>
    public class DefaultCommandQueue : ICommandQueue
    {
        /// <inheritdoc/>
        public DiscordBotBase Bot => _binder.Value;

        /// <summary>
        ///     Gets the degree of parallelism of this queue,
        ///     i.e. the amount of parallel command executions per-bucket.
        /// </summary>
        public int DegreeOfParallelism { get; }

        private Bucket _privateBucket;
        private readonly ISynchronizedDictionary<Snowflake, Bucket> _guildBuckets;

        private readonly Binder<DiscordBotBase> _binder;

        /// <summary>
        ///     Instantiates a new <see cref="DefaultCommandQueue"/>.
        /// </summary>
        /// <param name="options"> The options instance. </param>
        public DefaultCommandQueue(
            IOptions<DefaultCommandQueueConfiguration> options)
        {
            var configuration = options.Value;
            DegreeOfParallelism = configuration.DegreeOfParallelism;

            _guildBuckets = new SynchronizedDictionary<Snowflake, Bucket>();

            _binder = new Binder<DiscordBotBase>(this);
        }

        /// <inheritdoc/>
        public void Bind(DiscordBotBase value)
        {
            _binder.Bind(value);

            Bot.LeftGuild += LeftGuildAsync;
        }

        private Task LeftGuildAsync(object sender, LeftGuildEventArgs e)
        {
            if (_guildBuckets.TryRemove(e.GuildId, out var bucket))
                bucket.Complete();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Post(string input, DiscordCommandContext context, CommandQueueDelegate func)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Bucket kamaji;
            if (context.GuildId == null)
            {
                kamaji = _privateBucket ??= new Bucket(DegreeOfParallelism);
            }
            else
            {
                kamaji = _guildBuckets.GetOrAdd(context.GuildId.Value, static(_, queue) => new Bucket(queue.DegreeOfParallelism), this);
            }

            var bathToken = new Token(input, context, func);
            kamaji.Post(bathToken); // Thank you, mr boiler man!
        }

        private sealed class Token
        {
            public readonly DiscordCommandContext Context;
            private readonly string _input;
            private readonly CommandQueueDelegate _func;

            public Token(string input, DiscordCommandContext context, CommandQueueDelegate func)
            {
                _input = input;
                Context = context;
                _func = func;
            }

            public Task GetTask()
            {
                if (Context.Task != null)
                {
                    Context.ContinuationTcs.Complete();
                    Context.YieldTcs = new Tcs();
                }
                else
                {
                    Context.Task = _func(_input, Context);
                }

                return Task.WhenAny(Context.Task, Context.YieldTcs.Task);
            }
        }

        private sealed class Bucket
        {
            private readonly int _degreeOfParallelism;
            private readonly Channel<Token> _tokens;
            private readonly ISynchronizedDictionary<DiscordCommandContext, Task> _tasks;

            public Bucket(int degreeOfParallelism)
            {
                _degreeOfParallelism = degreeOfParallelism;
                _tokens = Channel.CreateUnbounded<Token>();
                _tasks = new SynchronizedDictionary<DiscordCommandContext, Task>(degreeOfParallelism);

                _ = RunAsync();
            }

            public void Post(Token token)
            {
                _tokens.Writer.TryWrite(token);
            }

            public void Complete()
            {
                _tokens.Writer.Complete();
            }

            private async Task RunAsync()
            {
                var reader = _tokens.Reader;
                await foreach (var token in reader.ReadAllAsync().ConfigureAwait(false))
                {
                    var tasks = _tasks.Values;
                    if (tasks.Length == _degreeOfParallelism)
                        await Task.WhenAny(tasks).ConfigureAwait(false);

                    var task = ExecuteTokenAsync(token);
                    _tasks.Add(token.Context, task);
                }
            }

            private async Task ExecuteTokenAsync(Token token)
            {
                await Task.Yield();
                var task = token.GetTask();
                await task.ConfigureAwait(false);
                _tasks.Remove(token.Context);
            }
        }
    }
}
