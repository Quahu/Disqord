using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class IntegrationCreateJsonModel : IntegrationJsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;
}