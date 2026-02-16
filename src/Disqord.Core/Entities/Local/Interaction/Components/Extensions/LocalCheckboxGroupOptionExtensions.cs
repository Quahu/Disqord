using Qommon;

namespace Disqord;

public static class LocalCheckboxGroupOptionExtensions
{
    public static LocalCheckboxGroupOption WithLabel(this LocalCheckboxGroupOption option, string label)
    {
        option.Label = label;
        return option;
    }

    public static LocalCheckboxGroupOption WithValue(this LocalCheckboxGroupOption option, string value)
    {
        option.Value = value;
        return option;
    }

    public static LocalCheckboxGroupOption WithDescription(this LocalCheckboxGroupOption option, string description)
    {
        option.Description = description;
        return option;
    }

    public static LocalCheckboxGroupOption WithIsDefault(this LocalCheckboxGroupOption option, bool isDefault = true)
    {
        option.IsDefault = isDefault;
        return option;
    }
}
