using Disqord.Models;

namespace Disqord
{
    public class MessageSticker : IPartialSticker
    {
        /// <inheritdoc/>
        public Snowflake Id { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
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
