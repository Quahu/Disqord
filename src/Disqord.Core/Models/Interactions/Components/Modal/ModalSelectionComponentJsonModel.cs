using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalSelectionComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("values")]
    public string[] Values = null!;

    public ModalSelectionComponentJsonModel()
    {
        Type = ComponentType.StringSelection;
    }
}
