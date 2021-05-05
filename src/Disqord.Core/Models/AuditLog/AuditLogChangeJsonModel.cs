using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogChangeJsonModel : JsonModel
    {
        [JsonProperty("new_value")]
        public Optional<IJsonNode> NewValue;

        [JsonProperty("old_value")]
        public Optional<IJsonNode> OldValue;

        [JsonProperty("key")]
        public string Key;
    }
}
