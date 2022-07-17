using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class ReorderJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("position")]
    public int Position;
}