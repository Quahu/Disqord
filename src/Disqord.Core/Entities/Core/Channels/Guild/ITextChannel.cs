namespace Disqord;

/// <summary>
///     Represents a guild text channel.
/// </summary>
public interface ITextChannel : IMessageGuildChannel, IThreadParentChannel, ITopicChannel
{
    /// <summary>
    ///     Gets whether this text channel is a news channel.
    /// </summary>
    bool IsNews { get; }
}