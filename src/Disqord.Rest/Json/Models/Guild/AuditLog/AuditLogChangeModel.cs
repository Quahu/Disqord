using System.Text.Json;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AuditLogChangeModel : JsonModel
    {
        [JsonProperty("old_value")]
        public Optional<IJsonElement> OldValue { get; set; }

        [JsonProperty("new_value")]
        public Optional<IJsonElement> NewValue { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}