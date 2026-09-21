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

    [JsonProperty("target_type")]
    public Optional<InviteTargetType> TargetType;

    [JsonProperty("target_user_id")]
    public Optional<Snowflake> TargetUserId;

    [JsonProperty("target_application_id")]
    public Optional<Snowflake> TargetApplicationId;

    [JsonProperty("target_user_ids")]
    public Optional<Snowflake[]> TargetUserIds;

    [JsonProperty("role_ids")]
    public Optional<Snowflake[]> RoleIds;

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(TargetUserIds, static targetUserIds => Guard.HasSizeLessThanOrEqualTo(targetUserIds, Discord.Limits.Rest.MaxInviteTargetUserAmount));
    }
}
