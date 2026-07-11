using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class SetVoiceChannelStatusJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("status")]
    public string? Status;
}
