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
            return extension.WaitForMessageAsync(context.ChannelId, predicate + (e => e.ChannelId == context.ChannelId && e.Message.Author.Id == context.Author.Id), timeout, cancellationToken);
        }
    }
}
