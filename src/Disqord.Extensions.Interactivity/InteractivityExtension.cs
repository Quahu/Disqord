using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Extensions.Interactivity
{
    public class InteractivityExtension : DiscordClientExtension
    {
        public TimeSpan DefaultMessageTimeout { get; }

        public TimeSpan DefaultReactionTimeout { get; }

        // ChannelId -> Waiter
        private readonly ISynchronizedDictionary<Snowflake, LinkedList<Waiter<MessageReceivedEventArgs>>> _messageWaiters;

        // MessageId -> Waiter
        private readonly ISynchronizedDictionary<Snowflake, LinkedList<Waiter<ReactionAddedEventArgs>>> _reactionWaiters;

        public InteractivityExtension(
            IOptions<InteractivityExtensionConfiguration> options,
            ILogger<InteractivityExtension> logger)
            : base(logger)
        {
            var configuration = options.Value;
            DefaultMessageTimeout = configuration.DefaultMessageTimeout;
            DefaultReactionTimeout = configuration.DefaultReactionTimeout;

            _messageWaiters = new SynchronizedDictionary<Snowflake, LinkedList<Waiter<MessageReceivedEventArgs>>>();
            _reactionWaiters = new SynchronizedDictionary<Snowflake, LinkedList<Waiter<ReactionAddedEventArgs>>>();
        }

        /// <inheritdoc/>
        protected override ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += MessageReceivedAsync;
            Client.MessageDeleted += MessageDeletedAsync;

            Client.ReactionAdded += ReactionAddedAsync;
            Client.ReactionRemoved += ReactionRemovedAsync;
            Client.ReactionsCleared += ReactionsClearedAsync;

            return default;
        }

        public async Task<MessageReceivedEventArgs> WaitForMessageAsync(
            Snowflake channelId, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            timeout = timeout != default
                ? timeout
                : DefaultMessageTimeout;
            using (var cts = Cts.Linked(Client.StoppingToken, cancellationToken))
            using (var waiter = new Waiter<MessageReceivedEventArgs>(predicate, timeout, cts.Token))
            {
                var waiters = _messageWaiters.GetOrAdd(channelId, _ => new LinkedList<Waiter<MessageReceivedEventArgs>>());
                lock (waiters)
                {
                    waiters.AddLast(waiter);
                }

                try
                {
                    return await waiter.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken != cts.Token) // Only checks for timeout.
                {
                    return null;
                }
            }
        }

        public async Task<ReactionAddedEventArgs> WaitForReactionAsync(
            Snowflake messageId, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            timeout = timeout != default
                ? timeout
                : DefaultReactionTimeout;
            using (var cts = Cts.Linked(Client.StoppingToken, cancellationToken))
            using (var waiter = new Waiter<ReactionAddedEventArgs>(predicate, timeout, cts.Token))
            {
                var waiters = _reactionWaiters.GetOrAdd(messageId, _ => new LinkedList<Waiter<ReactionAddedEventArgs>>());
                lock (waiters)
                {
                    waiters.AddLast(waiter);
                }

                try
                {
                    return await waiter.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken != cts.Token) // Only checks for timeout.
                {
                    return null;
                }
            }
        }

        private Task MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message.Author.IsBot)
                return Task.CompletedTask;

            if (_messageWaiters.TryGetValue(e.ChannelId, out var waiters))
            {
                lock (waiters)
                {
                    for (var current = waiters.First; current != null;)
                    {
                        if (current.Value.TryComplete(e))
                        {
                            var next = current.Next;
                            waiters.Remove(current);
                            current = next;
                            continue;
                        }

                        current = current.Next;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task MessageDeletedAsync(object sender, MessageDeletedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task ReactionAddedAsync(object sender, ReactionAddedEventArgs e)
        {
            if (e.Member.IsBot)
                return Task.CompletedTask;

            if (_reactionWaiters.TryGetValue(e.MessageId, out var waiters))
            {
                lock (waiters)
                {
                    for (var current = waiters.First; current != null;)
                    {
                        if (current.Value.TryComplete(e))
                        {
                            var next = current.Next;
                            waiters.Remove(current);
                            current = next;
                            continue;
                        }

                        current = current.Next;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task ReactionRemovedAsync(object sender, ReactionRemovedEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task ReactionsClearedAsync(object sender, ReactionsClearedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
