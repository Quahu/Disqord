using System.IO;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyCurrentMemberJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("nick")]
    public Optional<string?> Nick;

    [JsonProperty("avatar")]
    public Optional<Stream?> Avatar;

    [JsonProperty("banner")]
    public Optional<Stream?> Banner;

    [JsonProperty("bio")]
    public Optional<string?> Bio;
}
