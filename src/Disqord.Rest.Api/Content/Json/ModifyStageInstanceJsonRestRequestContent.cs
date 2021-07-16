using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyStageInstanceJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("topic")]
        public Optional<string> Topic;

        [JsonProperty("privacy_level")]
        public Optional<PrivacyLevel> PrivacyLevel;
    }
}
