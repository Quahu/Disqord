using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandOptionJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public ApplicationCommandOptionType Type;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("required")]
        public Optional<bool> Required;

        [JsonProperty("choices")]
        public Optional<ApplicationCommandOptionChoiceJsonModel[]> Choices;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;
    }
}
