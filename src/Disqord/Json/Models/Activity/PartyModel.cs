using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class PartyModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public int[] Size { get; set; }
    }
}