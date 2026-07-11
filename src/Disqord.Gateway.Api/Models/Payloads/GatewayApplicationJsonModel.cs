using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

[JsonSkippedProperties("flags")]
public class GatewayApplicationJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("flags_new")]
    public ApplicationFlags Flags;
}
