using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class MessageApplicationJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("cover_image")]
        public Optional<string> CoverImage;

        [JsonProperty("description")]
        public string Description = default!;

        [JsonProperty("icon")]
        public string Icon = default!;

        [JsonProperty("name")]
        public string Name = default!;
    }
}
