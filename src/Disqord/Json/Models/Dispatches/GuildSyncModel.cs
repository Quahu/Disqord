using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class GuildSyncModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("presences")]
        public PresenceUpdateModel[] Presences { get; set; }

        [JsonProperty("members")]
        public MemberModel[] Members { get; set; }
    }
}
