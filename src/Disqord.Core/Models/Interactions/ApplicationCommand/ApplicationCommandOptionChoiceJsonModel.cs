using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandOptionChoiceJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public IJsonValue Value;
    }
}