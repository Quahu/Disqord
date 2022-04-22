using Disqord.Serialization.Json;
using Qommon;

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
