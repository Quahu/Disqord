using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class CreateApplicationEmojiJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public string Name;

    [JsonProperty("image")]
    public Stream Image;

    public CreateApplicationEmojiJsonRestRequestContent(string name, Stream image)
    {
        Name = name;
        Image = image;
    }
}