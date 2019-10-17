using System;

namespace Disqord
{
    public partial interface IMessageChannel : IChannel, IMessagable
    {
        Snowflake? LastMessageId { get; }

        DateTimeOffset? LastPinTimestamp { get; }
    }
}
