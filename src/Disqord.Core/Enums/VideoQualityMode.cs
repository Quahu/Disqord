namespace Disqord;

/// <summary>
///     Represents the camera video quality mode of the voice channel.
/// </summary>
public enum VideoQualityMode
{
    /// <summary>
    ///     Discord chooses the quality for optimal performance.
    /// </summary>
    Automatic = 1,

    /// <summary>
    ///     Full video quality (720p).
    /// </summary>
    Full = 2
}