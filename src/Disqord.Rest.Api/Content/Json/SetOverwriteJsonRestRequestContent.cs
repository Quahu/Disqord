using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class SetOverwriteJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("allow")]
    public Permissions Allow;

    [JsonProperty("deny")]
    public Permissions Deny;

    [JsonProperty("type")]
    public OverwriteTargetType Type;
}
