using Disqord.Serialization.Json;

namespace Disqord.Models;

public class BotGatewayJsonModel : GatewayJsonModel
{
    [JsonProperty("shards")]
    public int Shards;

    [JsonProperty("session_start_limit")]
    public SessionStartLimitJsonModel SessionStartLimit = null!;
}