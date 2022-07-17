using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateChannelInviteJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("max_age")]
    public int MaxAge;

    [JsonProperty("max_uses")]
    public int MaxUses;

    [JsonProperty("temporary")]
    public bool Temporary;

    [JsonProperty("unique")]
    public bool Unique;

    [JsonProperty("target_user")]
    public Optional<Snowflake> TargetUser;

    [JsonProperty("target_user_type")]
    public Optional<int> TargetUserType;
}
