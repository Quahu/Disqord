using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyCurrentMemberJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("nick")]
        public Optional<string> Nick;
    }
}
