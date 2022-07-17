using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class SetOwnNickJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("nick")]
    public Optional<string> Nick;
}