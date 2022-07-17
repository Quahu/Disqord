using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildRoleCreateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("role")]
    public RoleJsonModel Role = null!;
}