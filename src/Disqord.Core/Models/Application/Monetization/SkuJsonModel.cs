using Disqord.Serialization.Json;

namespace Disqord.Models;

public class SkuJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public SkuType Type;

    [JsonProperty("application_id")]
    public Snowflake ApplicationId;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("slug")]
    public string Slug = null!;

    [JsonProperty("flags")]
    public SkuFlags Flags;
}
