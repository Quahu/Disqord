using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalLabelComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("component")]
    public ModalBaseComponentJsonModel Component = null!; // string selection or text input

    public ModalLabelComponentJsonModel()
    {
        Type = ComponentType.Label;
    }
}
