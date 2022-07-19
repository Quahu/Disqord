using Disqord.Serialization.Json;

namespace Disqord.Models;

public class OverwriteJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public OverwriteTargetType Type;

    [JsonProperty("allow")]
    public Permissions Allow;

    [JsonProperty("deny")]
    public Permissions Deny;
}
