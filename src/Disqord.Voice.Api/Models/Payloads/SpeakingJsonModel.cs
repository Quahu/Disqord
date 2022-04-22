using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Voice.Api.Models
{
    public class SpeakingJsonModel : JsonModel
    {
        [JsonProperty("speaking")]
        public bool Speaking;

        [JsonProperty("delay")]
        public int Delay;

        [JsonProperty("ssrc")]
        public uint Ssrc;

        [JsonProperty("user_id")]
        public Optional<Snowflake> UserId;
    }
}
