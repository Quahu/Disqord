using Disqord.Rest;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class JsonErrorModel : JsonModel
    {
        [JsonProperty("code")]
        public JsonErrorCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
