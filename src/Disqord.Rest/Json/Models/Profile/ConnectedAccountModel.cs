using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class ConnectedAccountModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}
