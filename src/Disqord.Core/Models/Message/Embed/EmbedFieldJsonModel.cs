using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class EmbedFieldJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name = default!;

        [JsonProperty("value")]
        public string Value = default!;

        [JsonProperty("inline")]
        public Optional<bool> Inline;
    }
}