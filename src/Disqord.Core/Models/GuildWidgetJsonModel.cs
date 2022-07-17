using Disqord.Serialization.Json;

namespace Disqord.Models;

public class GuildWidgetJsonModel : JsonModel
{
    [JsonProperty("enabled")]
    public bool Enabled;

    [JsonProperty("channel_id")]
    public Snowflake? ChannelId;
}