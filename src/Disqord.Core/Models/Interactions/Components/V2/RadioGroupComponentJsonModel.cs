using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class RadioGroupComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("options")]
    public RadioGroupOptionJsonModel[] Options = null!;

    [JsonProperty("required")]
    public Optional<bool> Required;

    [JsonProperty("disabled")]
    public Optional<bool> Disabled;

    public RadioGroupComponentJsonModel()
    {
        Type = ComponentType.RadioGroup;
    }
}
