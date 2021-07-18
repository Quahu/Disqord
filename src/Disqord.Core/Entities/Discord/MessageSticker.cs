using Disqord.Models;

namespace Disqord
{
    public class MessageSticker : IPartialSticker
    {
        public Snowflake Id { get; }

        public string Name { get; }

        public StickerFormatType FormatType { get; }

        public MessageSticker(StickerItemJsonModel model)
        {
            Id = model.Id;
            Name = model.Name;
            FormatType = model.FormatType;
        }

        public override string ToString()
            => $"Sticker {Name} ({Id})";
    }
}
