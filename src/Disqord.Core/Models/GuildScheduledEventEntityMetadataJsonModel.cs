using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildScheduledEventEntityMetadataJsonModel : JsonModel
    {
        [JsonProperty("location")]
        public Optional<string> Location;
    }
}
