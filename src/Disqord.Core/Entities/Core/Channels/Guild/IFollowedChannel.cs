using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a followed news channel.
/// </summary>
public interface IFollowedChannel : IChannelEntity, IJsonUpdatable<FollowedChannelJsonModel>
{
    /// <summary>
    ///     Gets the ID of the created target webhook of this followed channel.
    /// </summary>
    Snowflake WebhookId { get; }
}
