namespace Disqord;

/// <summary>
///     Represents a Discord entity that might exist within a channel.
/// </summary>
public interface IPossiblyChannelEntity : IEntity
{
    /// <summary>
    ///     Gets the ID of the channel of this entity.
    /// </summary>
    /// <returns>
    ///     Returns the ID of the channel or <see langword="null"/>
    ///     if this entity has no channel attached to it.
    /// </returns>
    Snowflake? ChannelId { get; }
}
