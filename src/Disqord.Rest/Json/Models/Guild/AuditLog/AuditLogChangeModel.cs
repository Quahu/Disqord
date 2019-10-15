using System.Text.Json;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AuditLogChangeModel
    {
        [JsonProperty("old_value")]
        public Optional<object> OldValue { get; set; }

        [JsonProperty("new_value")]
        public Optional<object> NewValue { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}