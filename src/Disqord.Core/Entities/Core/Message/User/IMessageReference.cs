namespace Disqord;

/// <summary>
///     Represents a reference to another message.
/// </summary>
public interface IMessageReference : IChannelEntity, IPossiblyGuildEntity
{
    /// <summary>
    ///     Gets the reference type of the referenced message.
    /// </summary>
    MessageReferenceType Type { get; }

    /// <summary>
    ///     Gets the ID of the referenced message.
    /// </summary>
    Snowflake? MessageId { get; }
}
