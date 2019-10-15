using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class HelloModel
    {
        [JsonProperty("heartbeat_interval", NullValueHandling = NullValueHandling.Ignore)]
        public int HeartbeatInterval { get; set; }

        [JsonProperty("_trace", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Trace { get; set; }
    }
}
