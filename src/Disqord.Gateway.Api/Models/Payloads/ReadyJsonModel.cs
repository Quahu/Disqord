using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

[JsonSkippedProperties("private_channels")]
public class ReadyJsonModel : JsonModel
{
    [JsonProperty("v")]
    public int V;

    [JsonProperty("user")]
    public UserJsonModel User = null!;

    [JsonProperty("guilds")]
    public UnavailableGuildJsonModel[] Guilds = null!;

    [JsonProperty("session_id")]
    public string SessionId = null!;

    [JsonProperty("resume_gateway_url")]
    public string ResumeGatewayUrl = null!;

    [JsonProperty("shard")]
    public Optional<int[]> Shard = null!;

    [JsonProperty("application")]
    public GatewayApplicationJsonModel Application = null!;
}
