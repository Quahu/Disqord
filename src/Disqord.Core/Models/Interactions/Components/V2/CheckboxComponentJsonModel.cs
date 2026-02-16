using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class CheckboxComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("default")]
    public Optional<bool> Default;

    [JsonProperty("label")]
    public Optional<string> Label;

    [JsonProperty("disabled")]
    public Optional<bool> Disabled;

    public CheckboxComponentJsonModel()
    {
        Type = ComponentType.Checkbox;
    }
}
