namespace Disqord;

/// <summary>
///     Represents a guild forum channel.
/// </summary>
public interface IForumChannel : IMediaChannel
{
    /// <summary>
    ///     Gets the default layout of posts in this channel.
    /// </summary>
    ForumLayout DefaultLayout { get; }
}
