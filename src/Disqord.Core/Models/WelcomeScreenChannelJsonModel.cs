using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class WelcomeScreenChannelJsonModel : JsonModel
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("emoji_id")]
        public Snowflake? EmojiId;

        [JsonProperty("emoji_name")]
        public string EmojiName;
    }
}
