using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class SetOwnNickJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("nick")]
        public Optional<string> Nick;
    }
}
