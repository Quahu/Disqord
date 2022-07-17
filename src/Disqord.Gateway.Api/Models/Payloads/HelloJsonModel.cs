using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class HelloJsonModel : JsonModel
{
    [JsonProperty("heartbeat_interval")]
    public int HeartbeatInterval;
}