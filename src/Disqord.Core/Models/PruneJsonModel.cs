using Disqord.Serialization.Json;

namespace Disqord.Models;

public class PruneJsonModel : JsonModel
{
    [JsonProperty("pruned")]
    public int? Pruned;
}