using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildIntegrationsUpdateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}