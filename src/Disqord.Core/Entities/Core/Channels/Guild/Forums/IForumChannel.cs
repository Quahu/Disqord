namespace Disqord;

/// <summary>
///     Represents a guild forum channel.
/// </summary>
public interface IForumChannel : IThreadParentChannel, ITopicChannel, ISlowmodeChannel
{
    /// <summary>
    ///     Gets the default layout of posts in this channel.
    /// </summary>
    ForumLayout DefaultLayout { get; }
}
