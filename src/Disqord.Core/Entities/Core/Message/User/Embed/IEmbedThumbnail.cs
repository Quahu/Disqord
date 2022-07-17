namespace Disqord;

/// <summary>
///     Represents the thumbnail of an embed.
/// </summary>
public interface IEmbedThumbnail : IEntity
{
    /// <summary>
    ///     Gets the URL of this thumbnail.
    /// </summary>
    string Url { get; }

    /// <summary>
    ///     Gets the proxy URL of this thumbnail.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? ProxyUrl { get; }

    /// <summary>
    ///     Gts the width of this thumbnail.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Width { get; }

    /// <summary>
    ///     Gts the height of this thumbnail.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Height { get; }
}
