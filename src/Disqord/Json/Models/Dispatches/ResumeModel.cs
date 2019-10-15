using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class ResumeModel
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("session_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("seq", NullValueHandling = NullValueHandling.Ignore)]
        public int? Seq { get; set; }
    }
}
