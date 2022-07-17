using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ChannelMentionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("type")]
    public ChannelType Type;

    [JsonProperty("name")]
    public string Name = null!;
}
