using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateStageInstanceJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("topic")]
    public string Topic = null!;

    [JsonProperty("privacy_level")]
    public Optional<PrivacyLevel> PrivacyLevel;

    [JsonProperty("send_start_notification")]
    public Optional<bool> SendStartNotification;
}
