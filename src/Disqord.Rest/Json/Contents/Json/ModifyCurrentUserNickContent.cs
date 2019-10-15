using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyCurrentUserNickContent : JsonRequestContent
    {
        [JsonProperty("nick")]
        public Optional<string> Nick { get; set; }
    }
}
