using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class VoiceRegionJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("vip")]
        public bool Vip;

        [JsonProperty("optimal")]
        public bool Optimal;

        [JsonProperty("deprecated")]
        public bool Deprecated;

        [JsonProperty("custom")]
        public bool Custom;
    }
}
