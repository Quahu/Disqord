using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ConnectionJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public string Id = default!;

        [JsonProperty("name")]
        public string Name = default!;

        [JsonProperty("type")]
        public string Type = default!;

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
        public ConnectionVisibility Visibility;
    }
}
