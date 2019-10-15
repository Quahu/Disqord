using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class GuildMemberAddModel : MemberModel
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
    }
}
