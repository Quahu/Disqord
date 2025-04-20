using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ContainerComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("accent_color")]
    public Optional<int?> AccentColor;

    [JsonProperty("spoiler")]
    public Optional<bool> Spoiler;

    [JsonProperty("components")]
    public BaseComponentJsonModel[] Components = null!;

    public ContainerComponentJsonModel()
    {
        Type = ComponentType.Container;
    }
}
