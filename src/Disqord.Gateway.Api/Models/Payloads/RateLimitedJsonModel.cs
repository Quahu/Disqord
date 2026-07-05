using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class RateLimitedJsonModel : JsonModel
{
    [JsonProperty("opcode")]
    public GatewayPayloadOperation Opcode;

    [JsonProperty("retry_after")]
    public float RetryAfter;

    [JsonProperty("meta")]
    public IJsonObject? Meta;
}
