namespace Disqord;

/// <summary>
///     Represents the format of a Discord CDN asset.
/// </summary>
public enum CdnAssetFormat : byte
{
    /// <summary>
    ///     No specific format. This makes Discord return the asset as-is, with no specific file extension.
    /// </summary>
    None,

    /// <summary>
    ///     The library will attempt to determine the best format for the given entity.
    /// </summary>
    Automatic,

    /// <summary>
    ///     The PNG format.
    /// </summary>
    Png,

    /// <summary>
    ///     The JPG (JPEG) format.
    /// </summary>
    Jpg,

    /// <summary>
    ///     The WebP format.
    /// </summary>
    WebP,

    /// <summary>
    ///     The GIF format.
    /// </summary>
    Gif
}