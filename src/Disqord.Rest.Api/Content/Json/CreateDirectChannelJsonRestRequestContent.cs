using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class CreateDirectChannelJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("recipient_id")]
    public Snowflake RecipientId;
}
