using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class StickerJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("format_type")]
        public StickerFormatType FormatType;
    }
}
