using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class SelectOptionJsonModel : JsonModel
    {
        [JsonProperty("label")]
        public string Label;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("emoji")]
        public Optional<EmojiJsonModel> Emoji;

        [JsonProperty("default")]
        public Optional<bool> Default;
    }
}
