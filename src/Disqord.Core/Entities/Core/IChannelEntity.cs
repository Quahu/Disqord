namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity that exists within a channel.
    /// </summary>
    public interface IChannelEntity : IEntity
    {
        /// <summary>
        ///     Gets the ID of the channel this entity is tied to.
        /// </summary>
        Snowflake ChannelId { get; }
    }
}
