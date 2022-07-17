using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class IntegrationDeleteJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("application_id")]
    public Optional<Snowflake> ApplicationId;
}