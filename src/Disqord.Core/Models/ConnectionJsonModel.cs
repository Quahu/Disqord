using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class ConnectionJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("revoked")]
        public Optional<bool> Revoked;

        [JsonProperty("integrations")]
        public Optional<IntegrationJsonModel[]> Integrations;

        [JsonProperty("verified")]
        public bool Verified;

        [JsonProperty("friend_sync")]
        public bool FriendSync;

        [JsonProperty("show_activity")]
        public bool ShowActivity;

        [JsonProperty("visibility")]
        public UserConnectionVisibility Visibility;
    }
}
