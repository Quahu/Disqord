using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class ResumeJsonModel : JsonModel
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("session_id")]
        public string SessionId;

        [JsonProperty("seq")]
        public int? Seq;
    }
}
