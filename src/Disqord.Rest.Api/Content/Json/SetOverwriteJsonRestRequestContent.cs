using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class SetOverwriteJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("allow")]
    public ulong Allow;

    [JsonProperty("deny")]
    public ulong Deny;

    [JsonProperty("type")]
    public OverwriteTargetType Type;
}