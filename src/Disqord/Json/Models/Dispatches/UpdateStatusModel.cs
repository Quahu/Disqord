using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class UpdateStatusModel
    {
        [JsonProperty("since")]
        public int? Since { get; set; }

        [JsonProperty("game")]
        public Optional<ActivityModel> Game { get; set; }

        [JsonProperty("status")]
        public Optional<UserStatus?> Status { get; set; }

        [JsonProperty("afk")]
        public bool Afk { get; set; }
    }
}
