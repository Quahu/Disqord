namespace Disqord;

/// <summary>
///     Represents the layout of posts in a forum channel.
/// </summary>
public enum ForumLayout : byte
{
    /// <summary>
    ///     No layout has been set.
    /// </summary>
    Unset = 0,

    /// <summary>
    ///     The posts are displayed as a chronological list.
    /// </summary>
    ListView = 1,

    /// <summary>
    ///     The posts are displayed as a collection of tiles.
    /// </summary>
    GalleryView = 2
}
