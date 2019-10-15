using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedFieldModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("inline")]
        public bool Inline { get; set; }
    }
}
