using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class SecretsModel
    {
        [JsonProperty("join", NullValueHandling.Ignore)]
        public string Join { get; set; }

        [JsonProperty("spectate", NullValueHandling.Ignore)]
        public string Spectate { get; set; }

        [JsonProperty("match", NullValueHandling.Ignore)]
        public string Match { get; set; }
    }
}