using Disqord.Models;

namespace Disqord
{
    public class TransientSticker : TransientEntity<StickerJsonModel>, ISticker
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public StickerFormatType FormatType => Model.FormatType;

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public string Tags => Model.Tags;

        /// <inheritdoc/>
        public StickerType Type => Model.Type;

        public TransientSticker(IClient client, StickerJsonModel model)
            : base(client, model)
        { }

        public static ISticker Create(IClient client, StickerJsonModel model)
            => model.Type switch
            {
                StickerType.Guild => new TransientGuildSticker(client, model),
                StickerType.Pack => new TransientPackSticker(client, model),
                _ => new TransientSticker(client, model)
            };
    }
}