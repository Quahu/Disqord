using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalCheckboxComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("value")]
    public bool Value;

    public ModalCheckboxComponentJsonModel()
    {
        Type = ComponentType.Checkbox;
    }
}
