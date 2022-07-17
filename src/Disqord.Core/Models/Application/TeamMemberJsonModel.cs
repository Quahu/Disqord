using Disqord.Serialization.Json;

namespace Disqord.Models;

public class TeamMemberJsonModel : JsonModel
{
    [JsonProperty("membership_state")]
    public TeamMembershipState MembershipState;

    [JsonProperty("permissions")]
    public string[] Permissions = null!;

    [JsonProperty("team_id")]
    public Snowflake TeamId;

    [JsonProperty("user")]
    public UserJsonModel User = null!;
}