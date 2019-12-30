using System;

namespace Disqord.Events
{
    public sealed class ChannelPinsUpdatedEventArgs : DiscordEventArgs
    {
        public ICachedMessageChannel Channel { get; }

        public DateTimeOffset? OldLastPinTimestamp { get; }

        public DateTimeOffset? NewLastPinTimestamp { get; }

        internal ChannelPinsUpdatedEventArgs(ICachedMessageChannel channel, DateTimeOffset? oldLastPinTimestamp) : base(channel.Client)
        {
            OldLastPinTimestamp = oldLastPinTimestamp;
            NewLastPinTimestamp = channel.LastPinTimestamp;
        }
    }
}
