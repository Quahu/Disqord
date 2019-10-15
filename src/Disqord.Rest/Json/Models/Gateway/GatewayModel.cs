using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class GatewayModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
