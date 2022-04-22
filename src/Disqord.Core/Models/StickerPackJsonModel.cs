using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class StickerPackJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("stickers")]
        public StickerJsonModel[] Stickers;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("sku_id")]
        public Snowflake SkuId;

        [JsonProperty("cover_sticker_id")]
        public Optional<Snowflake> CoverStickerId;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("banner_asset_id")]
        public Optional<Snowflake> BannerAssetId;
    }
}