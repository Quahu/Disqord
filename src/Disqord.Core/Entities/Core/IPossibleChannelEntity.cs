namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity that might exist within a channel.
    /// </summary>
    public interface IPossibleChannelEntity : IEntity
    {
        /// <summary>
        ///     Gets the ID of the channel of this entity.
        ///     Returns <see langword="null"/> if this entity has no channel attached to it.
        /// </summary>
        Snowflake? ChannelId { get; }
    }
}
