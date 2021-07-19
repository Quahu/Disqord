using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildDiscoveryCategoryJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public GuildDiscoveryCategoryNameJsonModel Name;

        [JsonProperty("is_primary")]
        public bool IsPrimary;
    }
}