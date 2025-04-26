using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ThumbnailComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("media")]
    public UnfurledMediaItemJsonModel Media = null!;

    [JsonProperty("description")]
    public Optional<string?> Description;

    [JsonProperty("spoiler")]
    public Optional<bool> Spoiler;

    public ThumbnailComponentJsonModel()
    {
        Type = ComponentType.Thumbnail;
    }
}
