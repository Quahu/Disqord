namespace Disqord;

/// <summary>
///     Represents a channel with a topic.
/// </summary>
public interface ITopicChannel : IGuildChannel
{
    /// <summary>
    ///     Gets the topic of this channel.
    /// </summary>
    string? Topic { get; }
}
