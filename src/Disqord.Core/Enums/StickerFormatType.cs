namespace Disqord;

/// <summary>
///     Represents the file format type of a sticker.
/// </summary>
public enum StickerFormatType : byte
{
    /// <summary>
    ///     The sticker is a PNG.
    /// </summary>
    Png = 1,

    /// <summary>
    ///     The sticker is an APNG.
    /// </summary>
    APng = 2,

    /// <summary>
    ///     The sticker is a Lottie.
    /// </summary>
    Lottie = 3
}