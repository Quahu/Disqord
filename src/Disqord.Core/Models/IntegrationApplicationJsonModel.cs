using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class IntegrationApplicationJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("summary")]
        public string Summary;

        [JsonProperty("bot")]
        public Optional<UserJsonModel> Bot;
    }
}
