using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateStageInstanceJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("topic")]
        public string Topic;

        [JsonProperty("privacy_level")]
        public Optional<PrivacyLevel> PrivacyLevel;
    }
}
