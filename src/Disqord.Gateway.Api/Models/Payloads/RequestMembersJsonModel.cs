using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class RequestMembersJsonModel : JsonModel
    {
        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("query")]
        public Optional<string> Query;

        [JsonProperty("limit")]
        public int Limit;

        [JsonProperty("presences")]
        public Optional<bool> Presences;

        [JsonProperty("user_ids")]
        public Optional<Snowflake[]> UserIds;

        [JsonProperty("nonce")]
        public Optional<string> Nonce;
    }
}
