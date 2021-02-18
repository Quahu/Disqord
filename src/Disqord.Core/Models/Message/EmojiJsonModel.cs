using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class EmojiJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake? Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("roles")]
        public Optional<Snowflake[]> Roles;

        [JsonProperty("user")]
        public Optional</*User*/JsonModel> User;

        [JsonProperty("require_colons")]
        public Optional<bool> RequireColons;

        [JsonProperty("managed")]
        public Optional<bool> Managed;

        [JsonProperty("animated")]
        public Optional<bool> Animated;

        [JsonProperty("available")]
        public Optional<bool> Available;
    }
}