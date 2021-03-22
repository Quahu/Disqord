using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Gateway;
using Disqord.Utilities;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Extensions.Interactivity
{
    public class InteractivityExtension : DiscordClientExtension
    {
        /// <summary>
        ///     Gets the default timeout used for awaiting messages.
        /// </summary>
        public TimeSpan DefaultMessageTimeout { get; }

        /// <summary>
        ///     Gets the default timeout used for awaiting reactions.
        /// </summary>
        public TimeSpan DefaultReactionTimeout { get; }

        /// <summary>
        ///     Gets the default timeout used for menus.
        /// </summary>
        public TimeSpan DefaultMenuTimeout { get; }

        // ChannelId -> Waiters
        private readonly ISynchronizedDictionary<Snowflake, LinkedList<Waiter<MessageReceivedEventArgs>>> _messageWaiters;

        // MessageId -> Waiters
        private readonly ISynchronizedDictionary<Snowflake, LinkedList<Waiter<ReactionAddedEventArgs>>> _reactionWaiters;

        // MessageId -> Menu
        private readonly ISynchronizedDictionary<Snowflake, MenuBase> _menus;

        public InteractivityExtension(
            IOptions<InteractivityExtensionConfiguration> options,
            ILogger<InteractivityExtension> logger)
            : base(logger)
        {
            var configuration = options.Value;
            DefaultMessageTimeout = configuration.DefaultMessageTimeout;
            DefaultReactionTimeout = configuration.DefaultReactionTimeout;
            DefaultMenuTimeout = configuration.DefaultMenuTimeout;

            _messageWaiters = new SynchronizedDictionary<Snowflake, LinkedList<Waiter<MessageReceivedEventArgs>>>();
            _reactionWaiters = new SynchronizedDictionary<Snowflake, LinkedList<Waiter<ReactionAddedEventArgs>>>();
            _menus = new SynchronizedDictionary<Snowflake, MenuBase>();
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
                catch (OperationCanceledException ex)
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
                catch (OperationCanceledException ex)
                {
                    return null;
                }
            }
        }

        public async Task StartMenuAsync(Snowflake channelId, MenuBase menu, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            timeout = timeout != default
                ? timeout
                : DefaultMenuTimeout;
            menu.Interactivity = this;
            menu.ChannelId = channelId;
            try
            {
                menu.MessageId = await menu.InitializeAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An exception occurred while attempting to initialise menu {menu.GetType()}.", ex);
            }

            if (!_menus.TryAdd(menu.MessageId, menu))
                throw new InvalidOperationException($"A menu with the message ID {menu.MessageId} is already added.");

            try
            {
                await menu.StartAsync(timeout, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An exception occurred while attempting to start menu {menu.GetType()}.", ex);
            }

            _ = InternalRunMenuAsync(menu);
        }

        public async Task RunMenuAsync(Snowflake channelId, MenuBase menu, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            if (!menu.IsRunning)
                await StartMenuAsync(channelId, menu, timeout, cancellationToken).ConfigureAwait(false);

            await InternalRunMenuAsync(menu).ConfigureAwait(false);
        }

        private async Task InternalRunMenuAsync(MenuBase menu)
        {
            try
            {
                await using (RuntimeDisposal.WrapAsync(menu, true).ConfigureAwait(false))
                {
                    await menu.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                _menus.Remove(menu.MessageId);
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

            if (_menus.TryGetValue(e.MessageId, out var menu))
                return menu.OnButtonAsync(new ButtonEventArgs(e));

            return Task.CompletedTask;
        }

        private Task ReactionRemovedAsync(object sender, ReactionRemovedEventArgs e)
        {
            if (_menus.TryGetValue(e.MessageId, out var menu))
                return menu.OnButtonAsync(new ButtonEventArgs(e));

            return Task.CompletedTask;
        }

        private Task ReactionsClearedAsync(object sender, ReactionsClearedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
