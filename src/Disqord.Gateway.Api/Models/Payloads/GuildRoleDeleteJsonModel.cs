using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildRoleDeleteJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("role_id")]
    public Snowflake RoleId;
}