using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class StickerJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("pack_id")]
        public Snowflake PackId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("tags")]
        public Optional<string> Tags;

        [JsonProperty("asset")]
        public string Asset;

        [JsonProperty("preview_asset")]
        public string PreviewAsset;

        [JsonProperty("format_type")]
        public StickerFormatType FormatType;
    }
}
