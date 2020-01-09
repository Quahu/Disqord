using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ConnectionModel : ConnectedAccountModel
    {
        [JsonProperty("revoked")]
        public bool Revoked { get; set; }

        [JsonProperty("integrations")]
        public IntegrationModel[] Integrations { get; set; }

        [JsonProperty("friend_sync")]
        public bool FriendSync { get; set; }

        [JsonProperty("show_activity")]
        public bool ShowActivity { get; set; }

        [JsonProperty("visibility")]
        public ConnectionVisibility Visibility { get; set; }
    }
}
