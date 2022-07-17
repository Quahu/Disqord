using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ReactionJsonModel : JsonModel
{
    [JsonProperty("count")]
    public int Count;

    [JsonProperty("me")]
    public bool Me;

    [JsonProperty("emoji")]
    public EmojiJsonModel Emoji = null!;
}
