using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class FileComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("file")]
    public UnfurledMediaItemJsonModel File = null!;

    [JsonProperty("spoiler")]
    public Optional<bool> Spoiler;

    public FileComponentJsonModel()
    {
        Type = ComponentType.File;
    }
}
