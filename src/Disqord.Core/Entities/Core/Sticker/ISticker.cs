using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a sticker.
    ///     This can be either an <see cref="IPackSticker"/> or <see cref="IGuildSticker"/>.
    /// </summary>
    public interface ISticker : IPartialSticker, IEntity, IJsonUpdatable<StickerJsonModel>
    {
        /// <summary>
        ///     Gets the description of this sticker.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the tags of this sticker.
        /// </summary>
        /// <remarks>
        ///     This returns different values based on the sticker type.
        ///     Guild stickers return the Discord name of the Unicode emoji.
        ///     Pack stickers return comma separated tags.
        /// </remarks>
        string Tags { get; }

        /// <summary>
        ///     Gets the type of this sticker.
        /// </summary>
        StickerType Type { get; }
    }
}