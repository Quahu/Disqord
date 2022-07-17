using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AutoModerationActionMetadataJsonModel : JsonModel
{
    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;

    [JsonProperty("duration_seconds")]
    public Optional<int> DurationSeconds;
}