using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ValidateSearchTermJsonModel : JsonModel
    {
        [JsonProperty("valid")]
        public bool Valid;
    }
}