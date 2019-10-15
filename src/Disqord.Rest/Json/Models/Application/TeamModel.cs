using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class TeamModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("owner_user_id")]
        public ulong OwnerUserId { get; set; }

        [JsonProperty("members")]
        public TeamMemberModel[] Members { get; set; }
    }
}
