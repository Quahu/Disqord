namespace Disqord;

public interface ITopicChannel : IGuildChannel
{
    /// <summary>
    ///     Gets the topic of this channel.
    /// </summary>
    string Topic { get; }
}
