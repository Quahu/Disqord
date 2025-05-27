using System;
using System.Linq;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ComponentJsonModel : BaseComponentJsonModel
{
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
    public Optional<BaseComponentJsonModel[]> Components;

    [JsonProperty("options")]
    public Optional<SelectOptionJsonModel[]> Options;

    [JsonProperty("channel_types")]
    public Optional<ChannelType[]> ChannelTypes;

    [JsonProperty("placeholder")]
    public Optional<string> Placeholder;

    [JsonProperty("default_values")]
    public Optional<DefaultValueJsonModel[]> DefaultValues;

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

    [JsonProperty("values")]
    public Optional<string[]> Values;

    protected override void OnValidate()
    {
        Guard.IsDefined(Type);

        if (Type == ComponentType.StringSelection || Type is >= ComponentType.UserSelection and <= ComponentType.ChannelSelection
            || Type == ComponentType.TextInput || Type == ComponentType.Button && Style != (byte) ButtonComponentStyle.Link)
        {
            OptionalGuard.HasValue(CustomId);
            Guard.IsNotNullOrWhiteSpace(CustomId.Value);
            Guard.IsLessThanOrEqualTo(CustomId.Value.Length, Discord.Limits.Component.MaxCustomIdLength);
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
                    Guard.IsLessThanOrEqualTo(label.Length, Discord.Limits.Component.Button.MaxLabelLength);
                });

                OptionalGuard.CheckValue(Emoji, emoji =>
                {
                    Guard.IsNotNull(emoji);
                });

                break;
            }
            case ComponentType.StringSelection or >= ComponentType.UserSelection and <= ComponentType.ChannelSelection:
            {
                if (Type == ComponentType.StringSelection)
                {
                    OptionalGuard.HasValue(Options);
                    Guard.HasSizeLessThanOrEqualTo(Options.Value, Discord.Limits.Component.Selection.MaxOptionAmount);

                    for (var i = 0; i < Options.Value.Length; i++)
                        Options.Value[i].Validate();
                }
                else
                {
                    OptionalGuard.CheckValue(Options, options =>
                    {
                        try
                        {
                            Guard.HasSizeEqualTo(options, 0);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Options are not supported by entity selection components.", ex);
                        }
                    });
                }

                OptionalGuard.CheckValue(Placeholder, placeholder =>
                {
                    Guard.IsNotNull(placeholder);
                    Guard.IsLessThanOrEqualTo(placeholder.Length, Discord.Limits.Component.Selection.MaxPlaceholderLength);
                });

                OptionalGuard.CheckValue(MinValues, minValues =>
                {
                    Guard.IsBetweenOrEqualTo(minValues, Discord.Limits.Component.Selection.MinMinimumSelectedOptions, Discord.Limits.Component.Selection.MaxMinimumSelectedOptions);
                });

                OptionalGuard.CheckValue(MaxValues, maxValues =>
                {
                    Guard.IsBetweenOrEqualTo(maxValues, Discord.Limits.Component.Selection.MinMaximumSelectedOptions, Discord.Limits.Component.Selection.MaxMaximumSelectedOptions);
                });

                if (MinValues.HasValue && MaxValues.HasValue)
                    Guard.IsLessThanOrEqualTo(MinValues.Value, MaxValues.Value);

                OptionalGuard.CheckValue(DefaultValues, defaultValues =>
                {
                    Guard.IsBetweenOrEqualTo(defaultValues.Length, MinValues.GetValueOrDefault(Discord.Limits.Component.Selection.MinMinimumSelectedOptions), MaxValues.GetValueOrDefault(Discord.Limits.Component.Selection.MaxMaximumSelectedOptions));

                    Predicate<DefaultValueJsonModel> predicate = Type switch
                    {
                        ComponentType.UserSelection => value => value.Type is DefaultSelectionValueType.User,
                        ComponentType.RoleSelection => value => value.Type is DefaultSelectionValueType.Role,
                        ComponentType.MentionableSelection => value => value.Type is DefaultSelectionValueType.User or DefaultSelectionValueType.Role,
                        ComponentType.ChannelSelection => value => value.Type is DefaultSelectionValueType.Channel,
                        _ => value => true
                    };

                    Guard.IsTrue(Array.TrueForAll(defaultValues, predicate), message: "The types of default selection values must match the type of the component.");
                });

                break;
            }
            case ComponentType.TextInput:
            {
                OptionalGuard.HasValue(Style);

                OptionalGuard.HasValue(Label);
                Guard.IsNotNullOrWhiteSpace(Label.Value);
                Guard.IsBetweenOrEqualTo(Label.Value.Length, Discord.Limits.Component.TextInput.MinLabelLength, Discord.Limits.Component.TextInput.MaxLabelLength);

                OptionalGuard.CheckValue(MinLength, minLength =>
                {
                    Guard.IsBetweenOrEqualTo(minLength, Discord.Limits.Component.TextInput.MinMinimumInputLength, Discord.Limits.Component.TextInput.MaxMinimumInputLength);
                });

                OptionalGuard.CheckValue(MaxLength, maxLength =>
                {
                    Guard.IsBetweenOrEqualTo(maxLength, Discord.Limits.Component.TextInput.MinMaximumInputLength, Discord.Limits.Component.TextInput.MaxMaximumInputLength);
                });

                if (MinLength.HasValue && MaxLength.HasValue)
                    Guard.IsLessThanOrEqualTo(MinLength.Value, MaxLength.Value);

                OptionalGuard.CheckValue(Value, value =>
                {
                    Guard.IsNotNull(value);
                    Guard.IsLessThanOrEqualTo(value.Length, Discord.Limits.Component.TextInput.MaxPrefilledValueLength);

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
                    Guard.IsLessThanOrEqualTo(placeholder.Length, Discord.Limits.Component.TextInput.MaxPlaceholderLength);
                });

                break;
            }
        }
    }
}
