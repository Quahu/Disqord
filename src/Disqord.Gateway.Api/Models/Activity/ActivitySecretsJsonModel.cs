using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class ActivitySecretsJsonModel : JsonModel
    {
        [JsonProperty("join")]
        public string Join;

        [JsonProperty("spectate")]
        public string Spectate;

        [JsonProperty("match")]
        public string Match;
    }
}
