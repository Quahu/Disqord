using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class GuildBanRemoveJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("user")]
    public UserJsonModel User = null!;
}