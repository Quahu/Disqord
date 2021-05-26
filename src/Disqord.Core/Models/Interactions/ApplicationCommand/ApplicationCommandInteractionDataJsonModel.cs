using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandInteractionDataJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("resolved")]
        public Optional<ApplicationCommandInteractionDataResolvedJsonModel> Resolved;

        [JsonProperty("name")]
        public Optional<ApplicationCommandInteractionDataOptionJsonModel[]> Options;

        [JsonProperty("custom_id")]
        public string CustomId;

        [JsonProperty("component_type")]
        public ComponentType ComponentType;
    }
}
