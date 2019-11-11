using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Events;

namespace Disqord.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityEntityExtensions
    {
        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this ICachedMessageChannel channel, Predicate<MessageReceivedEventArgs> predicate, TimeSpan timeout = default)
        {
            var extension = GetExtension(channel.Client);
            return extension.WaitForMessageAsync(predicate, timeout);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this ICachedMessageChannel channel, Predicate<ReactionAddedEventArgs> predicate, TimeSpan timeout = default)
        {
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
