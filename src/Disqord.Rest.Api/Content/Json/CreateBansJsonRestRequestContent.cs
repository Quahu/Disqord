using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateBansJsonRestRequestContent : CreateBanJsonRestRequestContent
{
    [JsonProperty("user_ids")]
    public IList<Snowflake> UserIds = null!;

    protected override void OnValidate()
    {
        base.OnValidate();

        Guard.IsNotNull(UserIds);
        Guard.HasSizeLessThanOrEqualTo(UserIds, Discord.Limits.Guild.MaxBulkBanUsersAmount);
    }
}
