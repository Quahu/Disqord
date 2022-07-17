namespace Disqord;

/// <summary>
///     Represents a file attached to a message.
/// </summary>
public interface IAttachment : IIdentifiableEntity
{
    /// <summary>
    ///     Gets the file name of this attachment.
    /// </summary>
    /// <remarks>
    ///     Includes the file extensions, e.g. <c>image.png</c>.
    /// </remarks>
    string FileName { get; }

    /// <summary>
    ///     Gets the media type of this attachment.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the media type of this attachment.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    string? ContentType { get; }

    /// <summary>
    ///     Gets the file size in bytes of this attachment.
    /// </summary>
    int FileSize { get; }

    /// <summary>
    ///     Gets the URL of this attachment.
    /// </summary>
    string Url { get; }

    /// <summary>
    ///     Gets the proxy URL of this attachment.
    /// </summary>
    string ProxyUrl { get; }

    /// <summary>
    ///     Gets the width of this attachment.
    ///     Returns <see langword="null"/> if this attachment was not an embedded image.
    /// </summary>
    int? Width { get; }

    /// <summary>
    ///     Gets the height of this attachment.
    ///     Returns <see langword="null"/> if this attachment was not an embedded image.
    /// </summary>
    int? Height { get; }

    /// <summary>
    ///     Gets whether this attachment is ephemeral, i.e. whether it is within an ephemeral message.
    /// </summary>
    bool IsEphemeral { get; }
}
