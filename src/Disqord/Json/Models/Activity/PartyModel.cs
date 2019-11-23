using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class PartyModel
    {
        [JsonProperty("id", NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("size", NullValueHandling.Ignore)]
        public int[] Size { get; set; }
    }
}