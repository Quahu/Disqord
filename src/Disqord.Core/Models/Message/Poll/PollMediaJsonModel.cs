using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class PollMediaJsonModel : JsonModel
{
    [JsonProperty("text")]
    public Optional<string> Text;

    [JsonProperty("emoji")]
    public Optional<EmojiJsonModel> Emoji;
}
