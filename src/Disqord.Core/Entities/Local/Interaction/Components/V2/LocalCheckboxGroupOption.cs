using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalCheckboxGroupOption : ILocalConstruct<LocalCheckboxGroupOption>, IJsonConvertible<CheckboxGroupOptionJsonModel>
{
    public Optional<string> Label { get; set; }

    public Optional<string> Value { get; set; }

    public Optional<string> Description { get; set; }

    public Optional<bool> IsDefault { get; set; }

    public LocalCheckboxGroupOption(string label, string value)
    {
        Label = label;
        Value = value;
    }

    public LocalCheckboxGroupOption()
    { }

    protected LocalCheckboxGroupOption(LocalCheckboxGroupOption other)
    {
        Label = other.Label;
        Value = other.Value;
        Description = other.Description;
        IsDefault = other.IsDefault;
    }

    public LocalCheckboxGroupOption Clone()
    {
        return new(this);
    }

    public CheckboxGroupOptionJsonModel ToModel()
    {
        OptionalGuard.HasValue(Label);
        OptionalGuard.HasValue(Value);

        return new CheckboxGroupOptionJsonModel
        {
            Label = Label.Value,
            Value = Value.Value,
            Description = Description,
            Default = IsDefault
        };
    }
}
