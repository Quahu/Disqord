using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityMenuEntityExtensions
    {
        public static Task StartMenuAsync(this ICachedMessageChannel channel, MenuBase menu, TimeSpan timeout = default)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            var extension = channel.Client.GetInteractivity();
            return extension.StartMenuAsync(channel, menu, timeout);
        }
    }
}
