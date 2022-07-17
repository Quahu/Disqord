using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class FollowNewsChannelJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("webhook_channel_id")]
    public Snowflake WebhookChannelId;
}