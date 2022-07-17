using Disqord.Serialization.Json;

namespace Disqord.Models;

public class StageInstanceJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("topic")]
    public string Topic = null!;

    [JsonProperty("privacy_level")]
    public PrivacyLevel PrivacyLevel;

    [JsonProperty("discoverable_disabled")]
    public bool DiscoverableDisabled;

    [JsonProperty("guild_scheduled_event_id")]
    public Snowflake? GuildScheduledEventId;
}
