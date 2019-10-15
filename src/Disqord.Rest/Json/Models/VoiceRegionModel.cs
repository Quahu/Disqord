using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class VoiceRegionModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vip")]
        public bool Vip { get; set; }

        [JsonProperty("optimal")]
        public bool Optimal { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("custom")]
        public bool Custom { get; set; }
    }
}
