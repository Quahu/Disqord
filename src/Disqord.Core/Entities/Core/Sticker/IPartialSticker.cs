namespace Disqord
{
    /// <summary>
    ///     Represents partial sticker data.
    /// </summary>
    public interface IPartialSticker : IEntity, IIdentifiableEntity, INamableEntity
    {
        /// <summary>
        ///     Gets the format type of this sticker.
        /// </summary>
        StickerFormatType FormatType { get; }
    }
}
