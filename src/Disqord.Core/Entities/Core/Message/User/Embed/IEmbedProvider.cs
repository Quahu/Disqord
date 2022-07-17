namespace Disqord;

/// <summary>
///     Represents the embed provider of an embed.
/// </summary>
public interface IEmbedProvider : IPossiblyNamableEntity
{
    /// <summary>
    ///     Gets the URL of this provider.
    ///     Returns <see langword="null"/> if not present.
    /// </summary>
    string? Url { get; }
}
