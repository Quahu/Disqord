using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildWidgetSettingsJsonModel : JsonModel
    {
        [JsonProperty("enabled")]
        public bool IsEnabled;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;
    }
}