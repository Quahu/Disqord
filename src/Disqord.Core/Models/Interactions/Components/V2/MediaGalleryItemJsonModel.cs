using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MediaGalleryItemJsonModel : JsonModel
{
    [JsonProperty("media")]
    public UnfurledMediaItemJsonModel Media = null!;

    [JsonProperty("description")]
    public Optional<string?> Description;

    [JsonProperty("spoiler")]
    public Optional<bool> Spoiler;
}
