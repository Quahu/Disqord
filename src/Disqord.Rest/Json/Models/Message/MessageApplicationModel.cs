using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class MessageApplicationModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("cover_image")]
        public Optional<string> CoverImage { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
