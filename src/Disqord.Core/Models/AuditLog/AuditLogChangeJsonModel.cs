using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class AuditLogChangeJsonModel : JsonModel
    {
        [JsonProperty("new_value")]
        public Optional<IJsonToken> NewValue;

        [JsonProperty("old_value")]
        public Optional<IJsonToken> OldValue;

        [JsonProperty("key")]
        public string Key;
    }
}
