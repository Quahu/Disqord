using Disqord.Models;

namespace Disqord
{
    public class Sticker
    {
        public Snowflake Id { get; }

        public Snowflake PackId { get; }

        public string Name { get; }

        public string Description { get; }

        public string Tags { get; }

        public string AssetHash { get; }

        public string PreviewAssetHash { get; }

        public StickerFormatType FormatType { get; }

        public Sticker(StickerJsonModel model)
        {
            Id = model.Id;
            PackId = model.PackId;
            Name = model.Name;
            Description = model.Description;
            Tags = model.Tags.GetValueOrDefault();
            AssetHash = model.Asset;
            PreviewAssetHash = model.PreviewAsset;
            FormatType = model.FormatType;
        }
    }
}
