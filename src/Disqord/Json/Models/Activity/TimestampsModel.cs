using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class TimestampsModel
    {
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public long? Start { get; set; }

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public long? End { get; set; }
    }
}
