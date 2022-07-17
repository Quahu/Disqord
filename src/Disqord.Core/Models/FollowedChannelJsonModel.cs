using Disqord.Serialization.Json;

namespace Disqord.Models;

public class FollowedChannelJsonModel : JsonModel
{
    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("webhook_id")]
    public Snowflake WebhookId;
}