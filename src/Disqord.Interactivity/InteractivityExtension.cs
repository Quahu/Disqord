using System;
using System.Threading.Tasks;
using Disqord.Events;

namespace Disqord.Interactivity
{
    public sealed class InteractivityExtension : DiscordClientExtension
    {
        public static readonly TimeSpan DefaultMessageTimeout = TimeSpan.FromSeconds(15);

        public static readonly TimeSpan DefaultReactionTimeout = TimeSpan.FromSeconds(15);

        public InteractivityExtension()
        { }

        protected override ValueTask SetupAsync()
            => default;

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
