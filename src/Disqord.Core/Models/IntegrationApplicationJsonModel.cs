using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class IntegrationApplicationJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name = default!;

        [JsonProperty("icon")]
        public string Icon = default!;

        [JsonProperty("description")]
        public string Description = default!;

        [JsonProperty("summary")]
        public string Summary = default!;

        [JsonProperty("bot")]
        public Optional<UserJsonModel> Bot;
    }
}
