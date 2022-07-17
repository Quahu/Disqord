using Disqord.Serialization.Json;

namespace Disqord.Models;

public class TeamJsonModel : JsonModel
{
    [JsonProperty("icon")]
    public string? Icon;

    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("members")]
    public TeamMemberJsonModel[] Members = null!;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("owner_user_id")]
    public Snowflake OwnerUserId;
}