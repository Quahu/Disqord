using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class InviteTargetUsersJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("user_ids")]
    public IList<Snowflake> UserIds = null!;

    protected override void OnValidate()
    {
        Guard.HasSizeLessThanOrEqualTo(UserIds, Discord.Limits.Rest.MaxInviteTargetUserAmount);
    }
}
