using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyCurrentMemberJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("nick")]
    public Optional<string> Nick;
}