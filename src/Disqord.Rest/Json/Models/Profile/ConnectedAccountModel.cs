using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ConnectedAccountModel
    {
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
