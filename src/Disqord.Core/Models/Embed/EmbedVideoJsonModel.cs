using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class EmbedVideoJsonModel : JsonModel
    {
        [JsonProperty("url")]
        public Optional<string> Url;

        [JsonProperty("height")]
        public Optional<int> IconUrl;

        [JsonProperty("width")]
        public Optional<int> ProxyIconUrl;
    }
}