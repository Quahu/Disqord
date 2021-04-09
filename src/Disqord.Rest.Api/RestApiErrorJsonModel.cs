using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public sealed class RestApiErrorJsonModel : JsonModel
    {
        [JsonProperty("message")]
        public Optional<string> Message;

        [JsonProperty("code")]
        public Optional<RestApiErrorCode> Code;
    }
}
