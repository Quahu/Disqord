using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class CheckboxGroupOptionJsonModel : JsonModel
{
    [JsonProperty("label")]
    public string Label = null!;

    [JsonProperty("value")]
    public string Value = null!;

    [JsonProperty("description")]
    public Optional<string> Description;

    [JsonProperty("default")]
    public Optional<bool> Default;
}
