using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a message channel.
    /// </summary>
    public interface IMessageChannel : IChannel
    {
        /// <summary>
        ///     Gets the last message ID of this channel.
        /// </summary>
        Snowflake? LastMessageId { get; }

        /// <summary>
        ///     Gets the last pin date of this channel.
        /// </summary>
        DateTimeOffset? LastPinTimestamp { get; }
    }
}
