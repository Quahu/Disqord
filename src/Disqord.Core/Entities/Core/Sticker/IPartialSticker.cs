namespace Disqord
{
    public interface IPartialSticker : IIdentifiable, INamable
    {
        StickerFormatType FormatType { get; }
    }
}