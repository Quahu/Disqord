using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandInteractionDataOptionJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public ApplicationCommandOptionType Type;

        [JsonProperty("value")]
        public Optional<ApplicationCommandOptionType> Value;

        [JsonProperty("options")]
        public Optional<ApplicationCommandInteractionDataOptionJsonModel[]> Options;
    }
}
