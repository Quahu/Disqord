using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class EmbedFieldJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("inline")]
        public Optional<bool> Inline;
    }
}