namespace Disqord;

/// <summary>
///     Represents a Discord entity that exists within a channel.
/// </summary>
public interface IChannelEntity : IPossiblyChannelEntity
{
    /// <summary>
    ///     Gets the ID of the channel this entity is tied to.
    /// </summary>
    new Snowflake ChannelId { get; }

    Snowflake? IPossiblyChannelEntity.ChannelId => ChannelId;
}
