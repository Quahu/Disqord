using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ComponentJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public ComponentType Type;
        
        [JsonProperty("style")]
        public Optional<ButtonComponentStyle> Interaction;
        
        [JsonProperty("label")]
        public Optional<string> Label;
        
        [JsonProperty("emoji")]
        public Optional<EmojiJsonModel> Emoji;
        
        [JsonProperty("string")]
        public Optional<string> CustomId;
        
        [JsonProperty("url")]
        public Optional<string> Url;
        
        [JsonProperty("disabled")]
        public Optional<bool> Disabled;
    }
}
