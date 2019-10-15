using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class AckMessageContent : JsonRequestContent
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
