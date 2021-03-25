using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Extensions.Interactivity.Menus;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityMenuEntityExtensions
    {
        public static Task StartMenuAsync(this IMessageChannel channel, MenuBase menu, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = channel.GetInteractivity();
            return extension.StartMenuAsync(channel.Id, menu, timeout, cancellationToken);
        }

        public static Task RunMenuAsync(this IMessageChannel channel, MenuBase menu, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            var extension = channel.GetInteractivity();
            return extension.RunMenuAsync(channel.Id, menu, timeout, cancellationToken);
        }
    }
}
