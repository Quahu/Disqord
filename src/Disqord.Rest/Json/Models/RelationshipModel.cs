using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class RelationshipModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("type")]
        public RelationshipType Type { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }
    }
}