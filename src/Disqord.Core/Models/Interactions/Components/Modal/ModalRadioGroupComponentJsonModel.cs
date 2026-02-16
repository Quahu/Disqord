using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ModalRadioGroupComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("value")]
    public Optional<string> Value;

    public ModalRadioGroupComponentJsonModel()
    {
        Type = ComponentType.RadioGroup;
    }
}
