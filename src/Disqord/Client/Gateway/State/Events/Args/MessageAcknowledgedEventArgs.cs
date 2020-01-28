using System;

namespace Disqord.Events
{
    public sealed class MessageAcknowledgedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public SnowflakeOptional<CachedMessage> Message { get; }

        internal MessageAcknowledgedEventArgs(ICachedMessageChannel channel, SnowflakeOptional<CachedMessage> message) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
        }
    }
}
