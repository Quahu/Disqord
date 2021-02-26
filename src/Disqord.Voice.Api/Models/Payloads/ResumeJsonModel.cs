using Disqord.Serialization.Json;

namespace Disqord.Voice.Api.Models
{
    public class ResumeJsonModel : JsonModel
    {
        [JsonProperty("server_id")]
        public Snowflake ServerId;

        [JsonProperty("session_id")]
        public string SessionId;

        [JsonProperty("token")]
        public string Token;
    }
}