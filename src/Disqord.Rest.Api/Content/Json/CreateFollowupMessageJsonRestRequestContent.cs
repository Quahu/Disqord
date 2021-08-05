using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateFollowupMessageJsonRestRequestContent : ExecuteWebhookJsonRestRequestContent
    {
        /// <summary>
        ///     Set to 64 to make your response ephemeral, <c>0oH4tvG70Eo</c>
        /// </summary>
        [JsonProperty("flags")]
        public Optional<InteractionResponseFlag> Flags;
    }
}
