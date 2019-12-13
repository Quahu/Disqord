using System;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Events;
using Disqord.Interactivity.Pagination;

namespace Disqord.Interactivity
{
    public sealed class InteractivityExtension : DiscordClientExtension
    {
        public static readonly TimeSpan DefaultMessageTimeout = TimeSpan.FromSeconds(15);

        public static readonly TimeSpan DefaultReactionTimeout = TimeSpan.FromSeconds(15);

        public static readonly TimeSpan DefaultPaginatorTimeout = TimeSpan.FromSeconds(30);

        private readonly LockedDictionary<Snowflake, PaginatorWrapper> _paginators = new LockedDictionary<Snowflake, PaginatorWrapper>();

        public InteractivityExtension()
        { }

        protected internal override ValueTask InitialiseAsync()
        {
            Client.ReactionAdded += ReactionAddedAsync;
            return default;
        }

        private async Task ReactionAddedAsync(ReactionAddedEventArgs e)
        {
            if (_paginators.TryGetValue(e.Message.Id, out var wrapper))
            {
                var page = await wrapper.Paginator.GetPageAsync(e);
                if (page != null)
                {
                    wrapper.Update();
                    await wrapper.Paginator.Message.ModifyAsync(x =>
                    {
                        x.Content = page.Content;
                        x.Embed = page.Embed;
                    }).ConfigureAwait(false);
                }
            }
        }

        public async Task SendPaginatorAsync(ICachedMessageChannel channel, PaginatorBase paginator, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            if (paginator == null)
                throw new ArgumentNullException(nameof(paginator));

            timeout = timeout == default
                ? DefaultPaginatorTimeout
                : timeout;

            paginator.Channel = channel;
            var defaultPage = paginator.DefaultPage;
            var message = await channel.SendMessageAsync(defaultPage.Content, embed: defaultPage.Embed).ConfigureAwait(false);
            _paginators.Add(message.Id, new PaginatorWrapper(this, paginator, timeout));
            paginator.Message = message;
            await paginator.InitialiseAsync().ConfigureAwait(false);
        }

        public async Task<PaginatorBase> ClosePaginatorAsync(Snowflake messageId)
        {
            if (!_paginators.TryRemove(messageId, out var wrapper))
                return null;

            await wrapper.DisposeAsync().ConfigureAwait(false);
            return wrapper.Paginator;
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
