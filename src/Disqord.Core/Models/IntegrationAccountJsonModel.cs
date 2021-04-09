using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class IntegrationAccountJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;
    }
}