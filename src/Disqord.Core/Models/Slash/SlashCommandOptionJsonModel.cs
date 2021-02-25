using Disqord.Serialization.Json;

namespace Disqord.Models.Slash
{
    public class SlashCommandOptionJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public SlashCommandOptionType Type;

        [JsonProperty("name")]
        public string Name = default!;

        [JsonProperty("description")]
        public string Description = default!;

        [JsonProperty("default")]
        public Optional<bool> Default;

        [JsonProperty("required")]
        public Optional<bool> Required;

        [JsonProperty("choices")]
        public Optional<SlashCommandOptionChoiceJsonModel[]> Choices;

        [JsonProperty("options")]
        public Optional<SlashCommandOptionJsonModel[]> Options;
    }
}