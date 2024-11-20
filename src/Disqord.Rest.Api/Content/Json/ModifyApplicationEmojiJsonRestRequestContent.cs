using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyApplicationEmojiJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public Optional<string> Name;
}