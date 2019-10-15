using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedProviderModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
