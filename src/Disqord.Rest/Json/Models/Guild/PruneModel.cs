using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class PruneModel : JsonModel
    {
        [JsonProperty("pruned")]
        public int? Pruned { get; set; }
    }
}
