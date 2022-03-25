using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class ComponentJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public ComponentType Type;

        [JsonProperty("style")]
        public Optional<byte> Style;

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

        [JsonProperty("min_length")]
        public Optional<int> MinLength;

        [JsonProperty("max_length")]
        public Optional<int> MaxLength;

        [JsonProperty("required")]
        public Optional<bool> Required;

        [JsonProperty("value")]
        public Optional<string> Value;

        protected override void OnValidate()
        {
            switch (Type)
            {
                case ComponentType.Row:
                {
                    OptionalGuard.CheckValue(Components, components =>
                    {
                        foreach (var component in components)
                        {
                            component.Validate();
                        }
                    });
                    break;
                }
                case ComponentType.TextInput:
                {
                    OptionalGuard.HasValue(CustomId);
                    OptionalGuard.HasValue(Label);

                    OptionalGuard.CheckValue(Value, value =>
                    {
                        Guard.IsNotNull(value);

                        OptionalGuard.CheckValue(MinLength, min =>
                        {
                            Guard.IsGreaterThanOrEqualTo(value.Length, min);
                        });

                        OptionalGuard.CheckValue(MaxLength, max =>
                        {
                            Guard.IsLessThanOrEqualTo(value.Length, max);
                        });
                    });
                    break;
                }
            }
        }
    }
}
