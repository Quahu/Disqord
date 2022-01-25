using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateFollowupMessageJsonRestRequestContent : ExecuteWebhookJsonRestRequestContent
    {
        [JsonProperty("flags")]
        public Optional<MessageFlag> Flags;
    }
}
