using System;
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
            Guard.IsDefined(Type);

            if (Type == ComponentType.Selection || Type == ComponentType.TextInput || Type == ComponentType.Button && Style != (byte) ButtonComponentStyle.Link)
            {
                OptionalGuard.HasValue(CustomId);
                Guard.IsNotNullOrWhiteSpace(CustomId.Value);
                Guard.IsLessThanOrEqualTo(CustomId.Value.Length, Discord.Limits.Components.MaxCustomIdLength);
            }

            switch (Type)
            {
                case ComponentType.Row:
                {
                    OptionalGuard.CheckValue(Components, components =>
                    {
                        for (var i = 0; i < components.Length; i++)
                            components[i].Validate();
                    });
                    break;
                }
                case ComponentType.Button:
                {
                    OptionalGuard.HasValue(Style);

                    if (Style.Value == (byte) ButtonComponentStyle.Link)
                    {
                        OptionalGuard.HasValue(Url);
                        Guard.IsNotNullOrWhiteSpace(Url.Value);
                    }

                    if (!Label.HasValue && !Emoji.HasValue)
                        throw new InvalidOperationException("The button's label or emoji must be set.");

                    OptionalGuard.CheckValue(Label, label =>
                    {
                        Guard.IsNotNullOrWhiteSpace(label);
                        Guard.IsLessThanOrEqualTo(label.Length, Discord.Limits.Components.Button.MaxLabelLength);
                    });

                    OptionalGuard.CheckValue(Emoji, emoji =>
                    {
                        Guard.IsNotNull(emoji);
                    });

                    break;
                }
                case ComponentType.Selection:
                {
                    OptionalGuard.HasValue(Options);
                    Guard.IsLessThanOrEqualTo(Options.Value.Length, Discord.Limits.Components.Selection.MaxOptionAmount);

                    for (var i = 0; i < Options.Value.Length; i++)
                        Options.Value[i].Validate();

                    OptionalGuard.CheckValue(Placeholder, placeholder =>
                    {
                        Guard.IsNotNull(placeholder);
                        Guard.IsLessThanOrEqualTo(placeholder.Length, Discord.Limits.Components.Selection.MaxPlaceholderLength);
                    });

                    OptionalGuard.CheckValue(MinValues, minValues =>
                    {
                        Guard.IsBetweenOrEqualTo(minValues, Discord.Limits.Components.Selection.MinMinimumValueAmount, Discord.Limits.Components.Selection.MaxMinimumValueAmount);
                    });

                    OptionalGuard.CheckValue(MaxValues, maxValues =>
                    {
                        Guard.IsBetweenOrEqualTo(maxValues, Discord.Limits.Components.Selection.MinMaximumValueAmount, Discord.Limits.Components.Selection.MaxMaximumValueAmount);
                    });

                    if (MinValues.HasValue && MaxValues.HasValue)
                        Guard.IsLessThanOrEqualTo(MinValues.Value, MaxValues.Value);

                    break;
                }
                case ComponentType.TextInput:
                {
                    OptionalGuard.HasValue(Style);

                    OptionalGuard.HasValue(Label);
                    Guard.IsNotNullOrWhiteSpace(Label.Value);
                    Guard.IsBetweenOrEqualTo(Label.Value.Length, Discord.Limits.Components.TextInput.MinLabelLength, Discord.Limits.Components.TextInput.MaxLabelLength);

                    OptionalGuard.CheckValue(MinLength, minLength =>
                    {
                        Guard.IsBetweenOrEqualTo(minLength, Discord.Limits.Components.TextInput.MinMinimumInputLength, Discord.Limits.Components.TextInput.MaxMinimumInputLength);
                    });

                    OptionalGuard.CheckValue(MaxLength, maxLength =>
                    {
                        Guard.IsBetweenOrEqualTo(maxLength, Discord.Limits.Components.TextInput.MinMaximumInputLength, Discord.Limits.Components.TextInput.MaxMaximumInputLength);
                    });

                    if (MinLength.HasValue && MaxLength.HasValue)
                        Guard.IsLessThanOrEqualTo(MinLength.Value, MaxLength.Value);

                    OptionalGuard.CheckValue(Value, value =>
                    {
                        Guard.IsNotNull(value);
                        Guard.IsLessThanOrEqualTo(value.Length, Discord.Limits.Components.TextInput.MaxPrefilledValueLength);

                        OptionalGuard.CheckValue(MinLength, minLength =>
                        {
                            Guard.IsGreaterThanOrEqualTo(value.Length, minLength);
                        });

                        OptionalGuard.CheckValue(MaxLength, maxLength =>
                        {
                            Guard.IsLessThanOrEqualTo(value.Length, maxLength);
                        });
                    });

                    OptionalGuard.CheckValue(Placeholder, placeholder =>
                    {
                        Guard.IsNotNull(placeholder);
                        Guard.IsLessThanOrEqualTo(placeholder.Length, Discord.Limits.Components.TextInput.MaxPlaceholderLength);
                    });
                    break;
                }
            }
        }
    }
}
