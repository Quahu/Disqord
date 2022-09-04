using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GatewayApplicationJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("flags")]
    public ApplicationFlags Flags;
}
