using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a sticker.
///     This can be either an <see cref="IPackSticker"/> or an <see cref="IGuildSticker"/>.
/// </summary>
public interface ISticker : IPartialSticker, IClientEntity, IJsonUpdatable<StickerJsonModel>
{
    /// <summary>
    ///     Gets the description of this sticker.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the tags of this sticker.
    /// </summary>
    /// <remarks>
    ///     This returns different values based on the sticker type.
    ///     Guild stickers return the Discord name of the Unicode emoji.
    ///     Pack stickers return comma separated tags the desktop client uses to find the sticker.
    /// </remarks>
    string Tags { get; }

    /// <summary>
    ///     Gets the type of this sticker.
    /// </summary>
    StickerType Type { get; }
}
