using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class MutualGuildModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }
    }
}
