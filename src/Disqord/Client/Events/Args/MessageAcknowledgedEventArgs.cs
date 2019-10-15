using System;

namespace Disqord.Events
{
    public sealed class MessageAcknowledgedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public OptionalSnowflakeEntity<CachedMessage> Message { get; }

        internal MessageAcknowledgedEventArgs(ICachedMessageChannel channel, OptionalSnowflakeEntity<CachedMessage> message) : base(channel.Client)
        {
            Channel = channel;
            Message = message;
        }
    }
}
