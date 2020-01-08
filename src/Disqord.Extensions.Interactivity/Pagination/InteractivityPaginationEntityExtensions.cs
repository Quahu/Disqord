using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Pagination
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityPaginationEntityExtensions
    {
        public static Task SendPaginatorAsync(this ICachedMessageChannel channel, PaginatorBase paginator, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            var extension = GetExtension(channel.Client);
            return extension.SendPaginatorAsync(channel, paginator, timeout);
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
