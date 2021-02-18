using System;

namespace Disqord.Gateway
{
    public class ChannelCreatedEventArgs : EventArgs
    {
        public IGuildChannel Channel { get; }

        public ChannelCreatedEventArgs(IGuildChannel channel)
        {
            Channel = channel;
        }
    }
}
