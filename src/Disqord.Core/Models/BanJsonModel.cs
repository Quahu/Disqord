using Disqord.Serialization.Json;

namespace Disqord.Models;

public class BanJsonModel : JsonModel
{
    [JsonProperty("reason")]
    public string? Reason;

    [JsonProperty("user")]
    public UserJsonModel User = null!;
}
