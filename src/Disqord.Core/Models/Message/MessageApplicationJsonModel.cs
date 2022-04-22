using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class MessageApplicationJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("cover_image")]
        public Optional<string> CoverImage;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("name")]
        public string Name;
    }
}
