using Disqord.Serialization.Json;

namespace Disqord.Models.Slash
{
    public class SlashCommandOptionChoiceJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public IJsonValue Value;
    }
}