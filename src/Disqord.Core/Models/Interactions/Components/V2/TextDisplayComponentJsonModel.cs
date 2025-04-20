using Disqord.Serialization.Json;

namespace Disqord.Models;

public class TextDisplayComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("content")]
    public string Content = null!;

    public TextDisplayComponentJsonModel()
    {
        Type = ComponentType.TextDisplay;
    }
}
