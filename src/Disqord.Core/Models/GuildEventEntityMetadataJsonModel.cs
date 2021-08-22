using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildEventEntityMetadataJsonModel : JsonModel
    {
        [JsonProperty("speaker_ids")]
        public Optional<Snowflake[]> SpeakerIds;
    }
}