using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class BanModel
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }
    }
}
