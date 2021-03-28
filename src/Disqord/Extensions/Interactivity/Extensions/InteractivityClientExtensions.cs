using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityClientExtensions
    {
        public static InteractivityExtension GetInteractivity(this DiscordClientBase client)
            => client.GetRequiredExtension<InteractivityExtension>();

        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this DiscordClientBase client, Snowflake channelId, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = client.GetInteractivity();
            return extension.WaitForMessageAsync(channelId, predicate, timeout, cancellationToken);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this DiscordClientBase client, Snowflake messageId, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = client.GetInteractivity();
            return extension.WaitForReactionAsync(messageId, predicate, timeout, cancellationToken);
        }
    }
}
