using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ComponentJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public ComponentType Type;

        [JsonProperty("style")]
        public Optional<ButtonComponentStyle> Style;

        [JsonProperty("label")]
        public Optional<string> Label;

        [JsonProperty("emoji")]
        public Optional<EmojiJsonModel> Emoji;

        [JsonProperty("custom_id")]
        public Optional<string> CustomId;

        [JsonProperty("url")]
        public Optional<string> Url;

        [JsonProperty("disabled")]
        public Optional<bool> Disabled;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;

        [JsonProperty("options")]
        public Optional<SelectOptionJsonModel[]> Options;

        [JsonProperty("placeholder")]
        public Optional<string> Placeholder;

        [JsonProperty("min_values")]
        public Optional<int> MinValues;

        [JsonProperty("max_values")]
        public Optional<int> MaxValues;
    }
}
