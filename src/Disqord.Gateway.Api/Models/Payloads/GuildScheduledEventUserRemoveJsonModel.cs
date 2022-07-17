using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildScheduledEventUserRemoveJsonModel : JsonModel
{
    [JsonProperty("guild_scheduled_event_id")]
    public Snowflake GuildScheduledEventId;

    [JsonProperty("user_id")]
    public Snowflake UserId;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}