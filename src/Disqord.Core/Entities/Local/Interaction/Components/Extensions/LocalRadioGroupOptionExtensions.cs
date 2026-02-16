using Qommon;

namespace Disqord;

public static class LocalRadioGroupOptionExtensions
{
    public static LocalRadioGroupOption WithLabel(this LocalRadioGroupOption option, string label)
    {
        option.Label = label;
        return option;
    }

    public static LocalRadioGroupOption WithValue(this LocalRadioGroupOption option, string value)
    {
        option.Value = value;
        return option;
    }

    public static LocalRadioGroupOption WithDescription(this LocalRadioGroupOption option, string description)
    {
        option.Description = description;
        return option;
    }

    public static LocalRadioGroupOption WithIsDefault(this LocalRadioGroupOption option, bool isDefault = true)
    {
        option.IsDefault = isDefault;
        return option;
    }
}
