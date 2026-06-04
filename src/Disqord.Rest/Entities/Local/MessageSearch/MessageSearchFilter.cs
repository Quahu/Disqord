namespace Disqord;

/// <summary>
///     Represents a filter for content types a message can contain.
/// </summary>
public enum MessageSearchFilter
{
    /// <summary>
    ///     The message contains a link.
    /// </summary>
    Link,

    /// <summary>
    ///     The message contains an embed.
    /// </summary>
    Embed,

    /// <summary>
    ///     The message contains a file attachment.
    /// </summary>
    File,

    /// <summary>
    ///     The message contains an image.
    /// </summary>
    Image,

    /// <summary>
    ///     The message contains a video.
    /// </summary>
    Video,

    /// <summary>
    ///     The message contains a sound.
    /// </summary>
    Sound,

    /// <summary>
    ///     The message contains a sticker.
    /// </summary>
    Sticker,

    /// <summary>
    ///     The message contains a poll.
    /// </summary>
    Poll,

    /// <summary>
    ///     The message contains a forward (message snapshot).
    /// </summary>
    Forward,
}
