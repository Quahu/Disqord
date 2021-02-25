using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class BanJsonModel : JsonModel
    {
        [JsonProperty("reason")]
        public string Reason = default!;

        [JsonProperty("user")]
        public UserJsonModel User = default!;
    }
}
