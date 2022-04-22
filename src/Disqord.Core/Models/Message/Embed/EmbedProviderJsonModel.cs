using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class EmbedProviderJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("url")]
        public Optional<string> Url;
    }
}