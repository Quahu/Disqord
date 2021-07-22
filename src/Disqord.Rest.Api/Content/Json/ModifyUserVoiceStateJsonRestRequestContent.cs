using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyMemberVoiceStateJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("suppress")]
        public Optional<bool> Suppress;
    }
}
