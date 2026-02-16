using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalRadioGroupOption : ILocalConstruct<LocalRadioGroupOption>, IJsonConvertible<RadioGroupOptionJsonModel>
{
    public Optional<string> Label { get; set; }

    public Optional<string> Value { get; set; }

    public Optional<string> Description { get; set; }

    public Optional<bool> IsDefault { get; set; }

    public LocalRadioGroupOption(string label, string value)
    {
        Label = label;
        Value = value;
    }

    public LocalRadioGroupOption()
    { }

    protected LocalRadioGroupOption(LocalRadioGroupOption other)
    {
        Label = other.Label;
        Value = other.Value;
        Description = other.Description;
        IsDefault = other.IsDefault;
    }

    public LocalRadioGroupOption Clone()
    {
        return new(this);
    }

    public RadioGroupOptionJsonModel ToModel()
    {
        OptionalGuard.HasValue(Label);
        OptionalGuard.HasValue(Value);

        return new RadioGroupOptionJsonModel
        {
            Label = Label.Value,
            Value = Value.Value,
            Description = Description,
            Default = IsDefault
        };
    }
}
