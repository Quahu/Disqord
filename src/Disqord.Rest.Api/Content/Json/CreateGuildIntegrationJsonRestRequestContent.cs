using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class CreateGuildIntegrationJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("type")]
    public string Type = null!;

    [JsonProperty("id")]
    public Snowflake Id;
}
