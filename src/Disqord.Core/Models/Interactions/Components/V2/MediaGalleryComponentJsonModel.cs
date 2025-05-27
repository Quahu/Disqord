using Disqord.Serialization.Json;

namespace Disqord.Models;

public class MediaGalleryComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("items")]
    public MediaGalleryItemJsonModel[] Items = null!;

    public MediaGalleryComponentJsonModel()
    {
        Type = ComponentType.MediaGallery;
    }
}
