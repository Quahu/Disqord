using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class LabelComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("label")]
    public string Label = null!;

    [JsonProperty("description")]
    public Optional<string> Description;

    [JsonProperty("component")]
    public BaseComponentJsonModel Component = null!; // string selection or text input

    public LabelComponentJsonModel()
    {
        Type = ComponentType.Label;
    }
}
