using System.IO;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateGuildFromTemplateJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public string Name;

    [JsonProperty("icon")]
    public Optional<Stream> Icon;

    public CreateGuildFromTemplateJsonRestRequestContent(string name)
    {
        Name = name;
    }
}