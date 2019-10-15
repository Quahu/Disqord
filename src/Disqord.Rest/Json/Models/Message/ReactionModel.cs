using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ReactionModel
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("me")]
        public bool Me { get; set; }

        [JsonProperty("emoji")]
        public EmojiModel Emoji { get; set; }
    }
}