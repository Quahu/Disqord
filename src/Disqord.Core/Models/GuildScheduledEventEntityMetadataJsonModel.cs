using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildScheduledEventEntityMetadataJsonModel : JsonModel
    {
        [JsonProperty("speaker_ids")]
        public Optional<Snowflake[]> SpeakerIds;

        [JsonProperty("location")]
        public Optional<string> Location;
    }
}
