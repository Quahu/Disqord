using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityBotExtensions
    {
        public static Task<MessageReceivedEventArgs> WaitForMessageAsync(this DiscordCommandContext context, Predicate<MessageReceivedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = context.Bot.GetInteractivity();
            return extension.WaitForMessageAsync(context.ChannelId, predicate, timeout, cancellationToken);
        }

        public static Task<ReactionAddedEventArgs> WaitForReactionAsync(this DiscordCommandContext context, Predicate<ReactionAddedEventArgs> predicate = null, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = context.Bot.GetInteractivity();
            return extension.WaitForReactionAsync(context.Message.Id, predicate, timeout, cancellationToken);
        }
    }
}
