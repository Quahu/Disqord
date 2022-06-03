using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AutoModerationActionJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public AutoModerationActionType Type;

        [JsonProperty("metadata")]
        public AutoModerationActionMetadataJsonModel Metadata;
    }
}
