using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public sealed class RestApiRateLimitedJsonModel : JsonModel
{
    [JsonProperty("retry_after")]
    public Optional<double> RetryAfter;
}
