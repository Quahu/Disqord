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
        internal static InteractivityExtension GetInteractivity(this IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not DiscordClientBase client)
                throw new InvalidOperationException("This entity's client is not a Discord client implementation.");

            return client.GetRequiredExtension<InteractivityExtension>();
        }

        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this IMessageChannel channel, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = channel.GetInteractivity();
            return extension.WaitForMessageAsync(channel.Id, predicate, timeout, cancellationToken);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this IMessageChannel channel, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = channel.GetInteractivity();
            return extension.WaitForReactionAsync(channel.Id, predicate, timeout, cancellationToken);
        }
    }
}
