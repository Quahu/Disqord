using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class EmbedVideoModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }
    }
}
