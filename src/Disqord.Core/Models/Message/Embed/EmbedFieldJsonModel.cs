using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class EmbedFieldJsonModel : JsonModel
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("value")]
    public string Value = null!;

    [JsonProperty("inline")]
    public Optional<bool> Inline;

    /// <inheritdoc />
    protected override void OnValidate()
    {
        Guard.IsNotNullOrWhiteSpace(Name);
        Guard.IsNotNullOrWhiteSpace(Value);
    }
}
