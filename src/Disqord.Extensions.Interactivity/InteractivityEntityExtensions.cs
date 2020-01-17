using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Events;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityEntityExtensions
    {
        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this ICachedMessageChannel channel, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Predicate<MessageReceivedEventArgs> channelPredicate = e => e.Message.Channel.Id == channel.Id;
            predicate = predicate == null
                ? channelPredicate
                : predicate + channelPredicate;

            var extension = channel.Client.GetInteractivity();
            return extension.WaitForMessageAsync(predicate, timeout);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this ICachedMessageChannel channel, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Predicate<ReactionAddedEventArgs> channelPredicate = e => e.Channel.Id == channel.Id;
            predicate = predicate == null
                ? channelPredicate
                : predicate + channelPredicate;

            var extension = channel.Client.GetInteractivity();
            return extension.WaitForReactionAsync(predicate, timeout);
        }

        public static InteractivityExtension GetInteractivity(this DiscordClientBase client)
        {
            var extension = client.GetExtension<InteractivityExtension>();
            if (extension == null)
                throw new InvalidOperationException("This client does not have the interactivity extension added.");

            return extension;
        }
    }
}
