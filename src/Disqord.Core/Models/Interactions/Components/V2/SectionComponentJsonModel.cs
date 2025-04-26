using Disqord.Serialization.Json;

namespace Disqord.Models;

public class SectionComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("components")]
    public BaseComponentJsonModel[] Components = null!;

    [JsonProperty("accessory")]
    public BaseComponentJsonModel Accessory = null!;

    public SectionComponentJsonModel()
    {
        Type = ComponentType.Section;
    }
}
