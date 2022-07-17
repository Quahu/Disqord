namespace Disqord;

/// <summary>
///     Represents the footer portion of an embed.
/// </summary>
public interface IEmbedFooter : IEntity
{
    /// <summary>
    ///     Gets the text of this footer.
    /// </summary>
    string Text { get; }

    /// <summary>
    ///     Gets the URL of the icon of this footer.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? IconUrl { get; }

    /// <summary>
    ///     Gets the proxy URL of the icon of this footer.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? ProxyIconUrl { get; }
}
