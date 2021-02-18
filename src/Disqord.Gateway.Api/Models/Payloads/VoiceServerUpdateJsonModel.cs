using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class VoiceServerUpdateJsonModel : JsonModel
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("guild_id")]
        public Snowflake GuildId;

        [JsonProperty("endpoint")]
        public string Endpoint;
    }
}
