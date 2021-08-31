using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityEntityExtensions
    {
        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this IMessageChannel channel, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = channel.GetDiscordClient().GetInteractivity();
            return extension.WaitForMessageAsync(channel.Id, predicate, timeout, cancellationToken);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this IMessage message, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = message.GetDiscordClient().GetInteractivity();
            return extension.WaitForReactionAsync(message.Id, predicate, timeout, cancellationToken);
        }
    }
}
