using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class IntegrationUpdateJsonModel : IntegrationJsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}