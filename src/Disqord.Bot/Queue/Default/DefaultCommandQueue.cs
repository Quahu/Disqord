using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qmmands.Text;
using Qommon;
using Qommon.Binding;
using Qommon.Collections.Synchronized;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Text;

/// <summary>
///     Represents the default implementation of an <see cref="ICommandQueue"/>.
///     Handles command execution based on the set degree of parallelism.
/// </summary>
public class DefaultCommandQueue : ICommandQueue
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public DiscordBotBase Bot => _binder.Value;

    /// <summary>
    ///     Gets the degree of parallelism of this queue,
    ///     i.e. the amount of parallel command executions per-bucket (per-guild).
    /// </summary>
    public int DegreeOfParallelism { get; }

    private Bucket? _privateBucket;
    private readonly ISynchronizedDictionary<Snowflake, Bucket> _guildBuckets;

    private readonly Binder<DiscordBotBase> _binder;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultCommandQueue"/>.
    /// </summary>
    /// <param name="options"> The options instance. </param>
    /// <param name="logger"> The logger of this queue. </param>
    public DefaultCommandQueue(
        IOptions<DefaultCommandQueueConfiguration> options,
        ILogger<DefaultCommandQueue> logger)
    {
        var configuration = options.Value;
        DegreeOfParallelism = configuration.DegreeOfParallelism;
        Logger = logger;

        _binder = new Binder<DiscordBotBase>(this);

        _guildBuckets = new SynchronizedDictionary<Snowflake, Bucket>();
    }

    /// <inheritdoc/>
    public void Bind(DiscordBotBase value)
    {
        _binder.Bind(value);

        Bot.LeftGuild += LeftGuildAsync;
    }

    private ValueTask LeftGuildAsync(object? sender, LeftGuildEventArgs e)
    {
        if (_guildBuckets.TryRemove(e.GuildId, out var bucket))
            bucket.Complete();

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public void Post(IDiscordTextCommandContext context, CommandQueueDelegate? func)
    {
        Guard.IsNotNull(context);

        Bucket kamaji;
        if (context.GuildId == null)
        {
            lock (this)
            {
                kamaji = _privateBucket ??= new Bucket(Logger, DegreeOfParallelism);
            }
        }
        else
        {
            kamaji = _guildBuckets.GetOrAdd(context.GuildId.Value, (_, queue) => new Bucket(queue.Logger, queue.DegreeOfParallelism), this);
        }

        var bathToken = new Token(context, func);
        kamaji.Post(bathToken); // Thank you, mr boiler man!
    }

    private sealed class Token
    {
        public readonly IDiscordTextCommandContext Context;

        public Task Task = null!;

        private readonly CommandQueueDelegate? _func;

        public Token(IDiscordTextCommandContext context, CommandQueueDelegate? func)
        {
            Context = context;
            _func = func;
        }

        public Task CreateTask()
        {
            Task task;
            if (Context.TryGetMetadata("KamajiTask", out task!))
            {
                Context.GetMetadata<Tcs>("KamajiContinuationTcs")!.Complete();

                // Context.SetMetadata("KamajiYieldTcs", new Tcs());
            }
            else
            {
                Context.SetMetadata("KamajiTask", task = _func!(Context));
            }

            var yieldTcs = new Tcs();
            Context.SetMetadata("KamajiYieldTcs", yieldTcs);
            return Task = Task.WhenAny(task, yieldTcs.Task);
        }
    }

    private sealed class Bucket
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(20);

        private readonly ILogger _logger;
        private readonly int _degreeOfParallelism;
        private readonly Channel<Token> _tokens;
        private readonly List<Token> _runningTokens;

        public Bucket(
            ILogger logger,
            int degreeOfParallelism)
        {
            _logger = logger;
            _degreeOfParallelism = degreeOfParallelism;
            _tokens = Channel.CreateUnbounded<Token>(new UnboundedChannelOptions
            {
                SingleReader = true,
            });

            _runningTokens = new List<Token>(degreeOfParallelism);

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
                Task task;
                try
                {
                    task = token.CreateTask();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception occurred while retrieving the task for the command queue token.");
                    continue;
                }

                if (task.IsCompleted)
                    continue;

                _runningTokens.Add(token);
                if (_runningTokens.Count == _degreeOfParallelism && _runningTokens.RemoveAll(x => x.Task.IsCompleted) == 0)
                {
                    using (var cts = new Cts(_timeout))
                    {
                        var timeoutTask = Task.Delay(-1, cts.Token);
                        var whenAnyTask = Task.WhenAny(_runningTokens.Select(x => x.Task));
                        var finishedTask = await Task.WhenAny(whenAnyTask, timeoutTask).ConfigureAwait(false);
                        if (finishedTask == timeoutTask)
                        {
                            _logger.LogWarning("The command queue has been blocked for over {0} seconds. "
                                + "Ensure that long-running work properly utilizes yielding and/or the parallel run mode. "
                                + "Commands blocking the queue: {1}.", _timeout.TotalSeconds, GetCommandPaths());

                            await whenAnyTask.ConfigureAwait(false);
                        }
                        else
                        {
                            cts.Cancel();
                        }
                    }

                    _runningTokens.RemoveAll(x => x.Task.IsCompleted);
                }
            }
        }

        private List<string> GetCommandPaths()
        {
            var commandPaths = new List<string>(_degreeOfParallelism);
            for (var i = 0; i < _degreeOfParallelism; i++)
            {
                var runningToken = _runningTokens[i];
                var path = runningToken.Context.Path;
                var command = runningToken.Context.Command;
                if (path == null && command == null)
                    continue;

                if (path != null)
                {
                    commandPaths.Add(string.Join(' ', path));
                    continue;
                }

                commandPaths.Add(command!.EnumerateFullAliases().First());
            }

            return commandPaths;
        }
    }
}
