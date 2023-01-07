namespace Disqord;

/// <summary>
///     Represents the sort order of posts in a forum channel.
/// </summary>
public enum ForumSortOrder : byte
{
    /// <summary>
    ///     The posts are sorted by activity.
    /// </summary>
    LatestActivity = 0,

    /// <summary>
    ///     The posts are sorted by creation date.
    /// </summary>
    CreationDate = 1
}
