using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models
{
    public class IdentifyJsonModel : JsonModel
    {
        [JsonProperty("server_id")]
        public Snowflake ServerId;

        [JsonProperty("user_id")]
        public Snowflake UserId;

        [JsonProperty("session_id")]
        public string SessionId;

        [JsonProperty("token")]
        public string Token;
    }
}
