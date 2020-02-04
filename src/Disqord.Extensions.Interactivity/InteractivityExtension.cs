using System;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Events;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Pagination;

namespace Disqord.Extensions.Interactivity
{
    public sealed class InteractivityExtension : DiscordClientExtension
    {
        public static readonly TimeSpan DefaultMessageTimeout = TimeSpan.FromSeconds(15);

        public static readonly TimeSpan DefaultReactionTimeout = TimeSpan.FromSeconds(15);

        public static readonly TimeSpan DefaultMenuTimeout = TimeSpan.FromMinutes(1);

        private readonly LockedDictionary<Snowflake, MenuWrapper> _menus = new LockedDictionary<Snowflake, MenuWrapper>();

        public InteractivityExtension()
        { }

        protected internal override ValueTask InitialiseAsync()
        {
            Client.ReactionAdded += ReactionAddedAsync;
            Client.ReactionRemoved += ReactionRemovedAsync;
            return default;
        }

        private Task ReactionAddedAsync(ReactionAddedEventArgs e)
        {
            if (e.User.Id == Client.CurrentUser.Id)
                return Task.CompletedTask;

            if (!_menus.TryGetValue(e.Message.Id, out var wrapper))
                return Task.CompletedTask;

            var args = new ButtonEventArgs(e);
            return wrapper.Menu.OnButtonAsync(args);
        }

        private Task ReactionRemovedAsync(ReactionRemovedEventArgs e)
        {
            if (e.User.Id == Client.CurrentUser.Id)
                return Task.CompletedTask;

            if (!_menus.TryGetValue(e.Message.Id, out var wrapper))
                return Task.CompletedTask;

            var args = new ButtonEventArgs(e);
            return wrapper.Menu.OnButtonAsync(args);
        }

        public async Task StartMenuAsync(IMessageChannel channel, MenuBase menu, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            timeout = timeout == default
                ? DefaultMenuTimeout
                : timeout;

            var wrapper = new MenuWrapper(this, menu, timeout);
            menu.Interactivity = this;
            menu.Channel = channel;
            menu._wrapper = wrapper;
            var message = await menu.InitialiseAsync().ConfigureAwait(false);
            if (message == null)
                throw new InvalidOperationException("Message returned from the menu's InitialiseAsync was null.");

            menu.Message = message;
            _menus.Add(message.Id, wrapper);
            await menu.StartAsync().ConfigureAwait(false);
        }

        public async Task<MenuBase> StopMenuAsync(Snowflake messageId)
        {
            if (!_menus.TryRemove(messageId, out var wrapper))
                return null;

            await wrapper.Menu.StopAsync().ConfigureAwait(false);
            return wrapper.Menu;
        }

        public async Task<MessageReceivedEventArgs> WaitForMessageAsync(Predicate<MessageReceivedEventArgs> predicate, TimeSpan timeout = default)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            timeout = timeout == default
                ? DefaultMessageTimeout
                : timeout;

            var tcs = new TaskCompletionSource<MessageReceivedEventArgs>();
            Task CheckAsync(MessageReceivedEventArgs e)
            {
                var invocationList = predicate.GetInvocationList();
                for (var i = 0; i < invocationList.Length; i++)
                {
                    var predicate = (Predicate<MessageReceivedEventArgs>) invocationList[i];
                    if (!predicate(e))
                        return Task.CompletedTask;
                }

                tcs.SetResult(e);
                return Task.CompletedTask;
            }

            Client.MessageReceived += CheckAsync;
            var task = tcs.Task;
            var delay = Task.Delay(timeout);
            var taskOrDelay = await Task.WhenAny(task, delay).ConfigureAwait(false);
            Client.MessageReceived -= CheckAsync;

            return taskOrDelay == task
                ? await task.ConfigureAwait(false)
                : null;
        }

        public async Task<ReactionAddedEventArgs> WaitForReactionAsync(Predicate<ReactionAddedEventArgs> predicate, TimeSpan timeout = default)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            timeout = timeout == default
                ? DefaultReactionTimeout
                : timeout;

            var tcs = new TaskCompletionSource<ReactionAddedEventArgs>();
            Task CheckAsync(ReactionAddedEventArgs e)
            {
                var invocationList = predicate.GetInvocationList();
                for (var i = 0; i < invocationList.Length; i++)
                {
                    var predicate = (Predicate<ReactionAddedEventArgs>) invocationList[i];
                    if (!predicate(e))
                        return Task.CompletedTask;
                }

                tcs.SetResult(e);
                return Task.CompletedTask;
            }

            Client.ReactionAdded += CheckAsync;
            var task = tcs.Task;
            var delay = Task.Delay(timeout);
            var taskOrDelay = await Task.WhenAny(task, delay).ConfigureAwait(false);
            Client.ReactionAdded -= CheckAsync;

            return taskOrDelay == task
                ? await task.ConfigureAwait(false)
                : null;
        }
    }
}
