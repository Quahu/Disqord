using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildDiscoveryMetadataJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("primary_category_id")]
        public Optional<int> PrimaryCategoryId;

        [JsonProperty("keywords")]
        public Optional<string[]> Keywords;

        [JsonProperty("emoji_discoverability_enabled")]
        public Optional<bool> EmojiDiscoverabilityEnabled;
    }
}