namespace Disqord;

/// <summary>
///     Represents the type of a sticker.
/// </summary>
public enum StickerType : byte
{
    /// <summary>
    ///     The sticker belongs to a sticker pack.
    /// </summary>
    Pack = 1,

    /// <summary>
    ///     The sticker belongs to a guild.
    /// </summary>
    Guild = 2
}