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

    [JsonProperty("shard")]
    public Optional<int[]> Shard = null!;
}