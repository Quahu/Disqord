namespace Disqord
{
    /// <summary>
    ///     Represents partial sticker data.
    /// </summary>
    public interface IPartialSticker : IIdentifiable, INamable
    {
        /// <summary>
        ///     Gets the format type of this sticker.
        /// </summary>
        StickerFormatType FormatType { get; }
    }
}