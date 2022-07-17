using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class WebhooksUpdateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("channel_id")]
    public Snowflake ChannelId;
}