using Disqord.Serialization.Json;

namespace Disqord.Models;

public class GuildBulkBanJsonModel : JsonModel
{
    [JsonProperty("banned_users")]
    public Snowflake[] BannedUsers = null!;

    [JsonProperty("failed_users")]
    public Snowflake[] FailedUsers = null!;
}
