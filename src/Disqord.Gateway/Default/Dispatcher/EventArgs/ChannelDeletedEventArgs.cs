using System;

namespace Disqord.Gateway
{
    public class ChannelDeletedEventArgs : EventArgs
    {
        public IGuildChannel Channel { get; }

        public ChannelDeletedEventArgs(IGuildChannel channel)
        {
            Channel = channel;
        }
    }
}
