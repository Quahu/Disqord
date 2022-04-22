using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class ModifyGuildIntegrationJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("expire_behavior")]
        public Optional<IntegrationExpirationBehavior> ExpireBehavior;

        [JsonProperty("expire_grace_period")]
        public Optional<int> ExpireGracePeriod;

        [JsonProperty("enable_emoticons")]
        public Optional<bool> EnableEmoticons;
    }
}
