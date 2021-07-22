using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyUserVoiceStateJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("suppress", NullValueHandling.Ignore)]
        public Optional<bool> Suppress;
    }
}
