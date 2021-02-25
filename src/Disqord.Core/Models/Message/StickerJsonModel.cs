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
        public string Name = default!;

        [JsonProperty("description")]
        public string Description = default!;

        [JsonProperty("tags")]
        public Optional<string> Tags;

        [JsonProperty("asset")]
        public string Asset = default!;

        [JsonProperty("preview_asset")]
        public string PreviewAsset = default!;

        [JsonProperty("format_type")]
        public StickerFormatType FormatType;
    }
}
