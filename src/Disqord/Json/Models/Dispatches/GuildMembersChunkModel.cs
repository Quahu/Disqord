using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class GuildMembersChunkModel
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        [JsonProperty("members")]
        public MemberModel[] Members { get; set; }

        [JsonProperty("presences")]
        public PresenceUpdateModel[] Presences { get; set; }
    }
}
