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

        public async Task StartMenuAsync(ICachedMessageChannel channel, MenuBase menu, bool wait = false, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            timeout = timeout == default
                ? DefaultMenuTimeout
                : timeout;

            var wrapper = new MenuWrapper(this, menu, timeout);
            menu._wrapper = wrapper;
            menu.Channel = channel;
            menu.MessageId = await menu.InitialiseAsync().ConfigureAwait(false);
            _menus.Add(menu.MessageId, wrapper);
            await menu.StartAsync(wrapper, channel, wait).ConfigureAwait(false);
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
            timeout = timeout == default
                ? DefaultMessageTimeout
                : timeout;

            var tcs = new TaskCompletionSource<MessageReceivedEventArgs>();
            Task CheckAsync(MessageReceivedEventArgs e)
            {
                if (predicate(e))
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
            timeout = timeout == default
                ? DefaultReactionTimeout
                : timeout;

            var tcs = new TaskCompletionSource<ReactionAddedEventArgs>();
            Task CheckAsync(ReactionAddedEventArgs e)
            {
                if (predicate(e))
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
