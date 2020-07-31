using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class GatewayModel : JsonModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
