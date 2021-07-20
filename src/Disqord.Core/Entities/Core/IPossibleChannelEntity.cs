namespace Disqord
{
    /// <summary>
    ///     Represents an entity that might have a channel ID attached to it.
    /// </summary>
    public interface IPossibleChannelEntity : IEntity
    {
        /// <summary>
        ///     Gets the optional ID of the channel of this entity.
        ///     Returns <see langword="null"/> if this entity has no channel attached to it.
        /// </summary>
        Snowflake? ChannelId { get; }
    }
}
