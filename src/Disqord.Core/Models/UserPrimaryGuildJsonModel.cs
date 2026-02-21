using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class UserPrimaryGuildJsonModel : JsonModel
{
    [JsonProperty("identity_guild_id")]
    public Snowflake? IdentityGuildId;

    [JsonProperty("identity_enabled")]
    public bool? IdentityEnabled;

    [JsonProperty("tag")]
    public string? Tag;

    [JsonProperty("badge")]
    public string? Badge;
}
