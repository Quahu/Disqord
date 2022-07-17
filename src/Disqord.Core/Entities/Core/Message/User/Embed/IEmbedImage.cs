namespace Disqord;

/// <summary>
///     Represents the image of an embed.
/// </summary>
public interface IEmbedImage : IEntity
{
    /// <summary>
    ///     Gets the URL of this image.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Url { get; }

    /// <summary>
    ///     Gets the proxy URL of this image.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? ProxyUrl { get; }

    /// <summary>
    ///     Gts the width of this image.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Width { get; }

    /// <summary>
    ///     Gts the height of this image.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Height { get; }
}
