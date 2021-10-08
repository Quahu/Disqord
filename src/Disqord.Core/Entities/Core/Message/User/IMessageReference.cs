namespace Disqord
{
    /// <summary>
    ///     Represents a reference to another message.
    /// </summary>
    public interface IMessageReference : IChannelEntity, IPossibleGuildEntity
    {
        /// <summary>
        ///     Gets the ID of the referenced message.
        /// </summary>
        Snowflake? MessageId { get; }
    }
}
