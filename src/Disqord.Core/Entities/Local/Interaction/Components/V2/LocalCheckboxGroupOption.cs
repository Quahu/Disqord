using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalCheckboxGroupOption : ILocalConstruct<LocalCheckboxGroupOption>, IJsonConvertible<CheckboxGroupOptionJsonModel>
{
    public string Label { get; set; }

    public string Value { get; set; }

    public Optional<string> Description { get; set; }

    public Optional<LocalEmoji> Emoji { get; set; }

    public Optional<bool> IsDefault { get; set; }

    public LocalCheckboxGroupOption(string label, string value)
    {
        Label = label;
        Value = value;
    }

    protected LocalCheckboxGroupOption(LocalCheckboxGroupOption other)
    {
        Label = other.Label;
        Value = other.Value;
        Description = other.Description;
        Emoji = other.Emoji;
        IsDefault = other.IsDefault;
    }

    public LocalCheckboxGroupOption Clone()
    {
        return new(this);
    }

    public CheckboxGroupOptionJsonModel ToModel()
    {
        return new CheckboxGroupOptionJsonModel
        {
            Label = Label,
            Value = Value,
            Description = Description,
            Emoji = Optional.Convert(Emoji, static emoji => emoji.ToModel()),
            Default = IsDefault
        };
    }
}
