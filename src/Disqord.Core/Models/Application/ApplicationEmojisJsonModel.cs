using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ApplicationEmojisJsonModel : JsonModel
{
    [JsonProperty("items")]
    public EmojiJsonModel[] Items = null!;
}