namespace Disqord;

/// <summary>
///     Represents the video of an embed.
/// </summary>
public interface IEmbedVideo : IEntity
{
    /// <summary>
    ///     Gets the URL of this video.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Url { get; }

    /// <summary>
    ///     Gts the width of this video.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Width { get; }

    /// <summary>
    ///     Gts the height of this video.
    ///     Returns <see langword="null"/> if unknown.
    /// </summary>
    int? Height { get; }
}
