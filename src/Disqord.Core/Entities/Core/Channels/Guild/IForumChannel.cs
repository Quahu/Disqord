namespace Disqord;

/// <summary>
///     Represents a guild forum channel.
/// </summary>
public interface IForumChannel : IThreadParentChannel, ITopicChannel, ISlowmodeChannel
{
    /// <summary>
    ///     Gets the ID of the last thread created in this channel.
    /// </summary>
    Snowflake? LastThreadId { get; }
}