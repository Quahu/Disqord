using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateBanJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("delete_message_seconds")]
    public Optional<int> DeleteMessageSeconds;

    [JsonProperty("reason")]
    public Optional<string> Reason;

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(DeleteMessageSeconds, seconds => Guard.IsBetweenOrEqualTo(seconds, 0, Discord.Limits.Guild.MaxDeleteMessageSeconds));
        OptionalGuard.CheckValue(Reason, static reason => Guard.HasSizeLessThanOrEqualTo(reason, Discord.Limits.Rest.MaxAuditLogReasonLength));
    }
}
