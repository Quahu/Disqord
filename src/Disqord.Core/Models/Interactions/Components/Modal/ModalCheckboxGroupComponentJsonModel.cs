using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalCheckboxGroupComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("values")]
    public string[] Values = null!;

    public ModalCheckboxGroupComponentJsonModel()
    {
        Type = ComponentType.CheckboxGroup;
    }
}
