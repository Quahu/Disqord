using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class CheckboxGroupComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("options")]
    public CheckboxGroupOptionJsonModel[] Options = null!;

    [JsonProperty("min_values")]
    public Optional<int> MinValues;

    [JsonProperty("max_values")]
    public Optional<int> MaxValues;

    [JsonProperty("required")]
    public Optional<bool> Required;

    [JsonProperty("disabled")]
    public Optional<bool> Disabled;

    public CheckboxGroupComponentJsonModel()
    {
        Type = ComponentType.CheckboxGroup;
    }
}
