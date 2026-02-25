using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateBanJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("delete_message_days")]
    public Optional<int> DeleteMessageDays;
    
    [JsonProperty("delete_message_seconds")]
    public Optional<int> DeleteMessageSeconds;

    [JsonProperty("reason")]
    public Optional<string> Reason;

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(DeleteMessageDays, static days => Guard.IsBetweenOrEqualTo(days, 0, Discord.Limits.Rest.BanDeleteMessageDays));
        OptionalGuard.CheckValue(DeleteMessageSeconds, static days => Guard.IsBetweenOrEqualTo(days, 0, Discord.Limits.Rest.BanDeleteMessageSeconds));
        OptionalGuard.CheckValue(Reason, static reason => Guard.HasSizeLessThanOrEqualTo(reason, Discord.Limits.Rest.MaxAuditLogReasonLength));
    }
}
