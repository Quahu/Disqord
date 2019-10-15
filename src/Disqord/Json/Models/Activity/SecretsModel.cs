using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class SecretsModel
    {
        [JsonProperty("join", NullValueHandling = NullValueHandling.Ignore)]
        public string Join { get; set; }

        [JsonProperty("spectate", NullValueHandling = NullValueHandling.Ignore)]
        public string Spectate { get; set; }

        [JsonProperty("match", NullValueHandling = NullValueHandling.Ignore)]
        public string Match { get; set; }
    }
}