using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class SeparatorComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("divider")]
    public Optional<bool> Divider;

    [JsonProperty("spacing")]
    public Optional<SeparatorComponentSpacingSize> Spacing;

    public SeparatorComponentJsonModel()
    {
        Type = ComponentType.Separator;
    }
}
