using Disqord.Models;

namespace Disqord
{
    public interface ISticker : IPartialSticker, IEntity, IJsonUpdatable<StickerJsonModel>
    {
        string Description { get; }

        string Tags { get; }

        StickerType Type { get; }
    }
}