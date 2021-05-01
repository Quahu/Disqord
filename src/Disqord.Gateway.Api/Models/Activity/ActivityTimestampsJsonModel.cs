using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class ActivityTimestampsJsonModel : JsonModel
    {
        [JsonProperty("start")]
        public Optional<long> Start;

        [JsonProperty("end")]
        public Optional<long> End;
    }
}
