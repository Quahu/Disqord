using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class GatewayBotModel : GatewayModel
    {
        [JsonProperty("shards")]
        public int Shards { get; set; }

        [JsonProperty("session_start_limit")]
        public SessionStartLimitModel SessionStartLimit { get; set; }
    }
}
