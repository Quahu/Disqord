using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class PartialAttachmentJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("description")]
        public Optional<string> Description;
    }
}
