namespace Disqord
{
    /// <summary>
    ///     Represents an entity existing within a channel.
    /// </summary>
    public interface IChannelEntity : IEntity
    {
        /// <summary>
        ///     Gets the channel ID this entity is tied to.
        /// </summary>
        Snowflake ChannelId { get; }
    }
}
