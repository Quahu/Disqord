using Disqord.Serialization.Json;

namespace Disqord.Models;

public class SessionStartLimitJsonModel : JsonModel
{
    [JsonProperty("total")]
    public int Total;

    [JsonProperty("remaining")]
    public int Remaining;

    [JsonProperty("reset_after")]
    public long ResetAfter;

    [JsonProperty("max_concurrency")]
    public int MaxConcurrency;
}