using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildAuditLogEntryCreateJsonModel : AuditLogEntryJsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}
