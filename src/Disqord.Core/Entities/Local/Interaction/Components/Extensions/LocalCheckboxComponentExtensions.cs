namespace Disqord;

public static class LocalCheckboxComponentExtensions
{
    public static TCheckboxComponent WithLabel<TCheckboxComponent>(this TCheckboxComponent checkboxComponent, string label)
        where TCheckboxComponent : LocalCheckboxComponent
    {
        checkboxComponent.Label = label;
        return checkboxComponent;
    }

    public static TCheckboxComponent WithIsDefault<TCheckboxComponent>(this TCheckboxComponent checkboxComponent, bool isDefault = true)
        where TCheckboxComponent : LocalCheckboxComponent
    {
        checkboxComponent.IsDefault = isDefault;
        return checkboxComponent;
    }

    public static TCheckboxComponent WithIsDisabled<TCheckboxComponent>(this TCheckboxComponent checkboxComponent, bool isDisabled = true)
        where TCheckboxComponent : LocalCheckboxComponent
    {
        checkboxComponent.IsDisabled = isDisabled;
        return checkboxComponent;
    }
}
