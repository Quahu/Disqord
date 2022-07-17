namespace Disqord;

/// <summary>
///     Represents the author portion of an embed.
/// </summary>
public interface IEmbedAuthor : INamableEntity
{
    /// <summary>
    ///     Gets the URL of this author.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Url { get; }

    /// <summary>
    ///     Gets the URL of the icon of this author.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? IconUrl { get; }

    /// <summary>
    ///     Gets the proxy URL of the icon of this author.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? ProxyIconUrl { get; }
}
