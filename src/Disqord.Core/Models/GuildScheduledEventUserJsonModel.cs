using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class GuildScheduledEventUserJsonModel : JsonModel
    {
        [JsonProperty("guild_scheduled_event_id")]
        public Snowflake GuildScheduledEventId;

        [JsonProperty("user")]
        public UserJsonModel User;

        [JsonProperty("member")]
        public Optional<MemberJsonModel> Member;
    }
}
