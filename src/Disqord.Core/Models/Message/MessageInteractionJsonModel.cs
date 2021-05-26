using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class MessageInteractionJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("type")]
        public InteractionType Type;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("user")]
        public UserJsonModel User;
    }
}
