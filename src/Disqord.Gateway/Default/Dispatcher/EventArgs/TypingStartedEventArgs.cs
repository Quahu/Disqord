using System;

namespace Disqord.Gateway
{
    public class TypingStartedEventArgs : EventArgs
    {
        public Snowflake ChannelId { get; }

        public TypingStartedEventArgs(Snowflake channelId)
        {
            ChannelId = channelId;
        }
    }
}
