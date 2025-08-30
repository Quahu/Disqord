using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalTextInputComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("value")]
    public string Value = null!;

    public ModalTextInputComponentJsonModel()
    {
        Type = ComponentType.TextInput;
    }
}
