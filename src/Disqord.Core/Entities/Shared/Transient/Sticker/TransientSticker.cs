using Disqord.Models;

namespace Disqord
{
    public class TransientSticker : TransientEntity<StickerJsonModel>, ISticker
    {
        public Snowflake Id => Model.Id;

        public string Name => Model.Name;

        public StickerFormatType FormatType => Model.FormatType;

        public string Description => Model.Description;

        public string Tags => Model.Tags;

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