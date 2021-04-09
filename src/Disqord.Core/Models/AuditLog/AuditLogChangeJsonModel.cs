using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogChangeJsonModel : JsonModel
    {
        [JsonProperty("old_value")]
        public Optional<IJsonToken> OldValue;

        [JsonProperty("new_value")]
        public Optional<IJsonToken> NewValue;

        [JsonProperty("key")]
        public string Key;
    }
}