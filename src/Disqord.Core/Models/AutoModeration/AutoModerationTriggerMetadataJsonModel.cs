using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class AutoModerationTriggerMetadataJsonModel : JsonModel
    {
        [JsonProperty("keyword_filter")]
        public Optional<string[]> KeywordFilter;

        // TODO: Change to AutoModerationWordsetType when migrated in the api
        [JsonProperty("keyword_lists")]
        public Optional<string[]> KeywordLists;
    }
}
