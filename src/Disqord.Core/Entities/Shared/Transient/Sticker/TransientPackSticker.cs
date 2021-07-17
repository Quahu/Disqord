using Disqord.Models;

namespace Disqord
{
    public class TransientPackSticker : TransientSticker, IPackSticker
    {
        public Snowflake PackId => Model.PackId.Value;

        public int SortValue => Model.SortValue.Value;

        public TransientPackSticker(IClient client, StickerJsonModel model)
            : base(client, model)
        { }
    }
}