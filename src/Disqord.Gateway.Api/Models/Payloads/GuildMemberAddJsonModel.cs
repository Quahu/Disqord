using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildMemberAddJsonModel : MemberJsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}