using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class SessionStartLimitModel
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("reset_after")]
        public int ResetAfter { get; set; }
    }
}