using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class IntegrationAccountJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public string Id = default!;

        [JsonProperty("name")]
        public string Name = default!;
    }
}