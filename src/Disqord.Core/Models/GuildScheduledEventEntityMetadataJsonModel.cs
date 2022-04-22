using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class GuildScheduledEventEntityMetadataJsonModel : JsonModel
    {
        [JsonProperty("location")]
        public Optional<string> Location;
    }
}
