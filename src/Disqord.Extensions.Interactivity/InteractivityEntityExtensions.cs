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

            var extension = GetExtension(channel.Client);
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

            var extension = GetExtension(channel.Client);
            return extension.WaitForReactionAsync(predicate, timeout);
        }

        private static InteractivityExtension GetExtension(DiscordClientBase client)
        {
            var extension = client.GetExtension<InteractivityExtension>();
            if (extension == null)
                throw new InvalidOperationException("This client does not have the interactivity extension added.");

            return extension;
        }
    }
}
