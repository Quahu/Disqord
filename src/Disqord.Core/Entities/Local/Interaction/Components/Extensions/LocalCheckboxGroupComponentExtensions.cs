using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalCheckboxGroupComponentExtensions
{
    public static TCheckboxGroupComponent AddOption<TCheckboxGroupComponent>(this TCheckboxGroupComponent component, string label, string value, string? description = null, bool isDefault = false)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        var option = new LocalCheckboxGroupOption(label, value)
        {
            IsDefault = isDefault
        };

        if (description != null)
            option.Description = description;

        return component.AddOption(option);
    }

    public static TCheckboxGroupComponent AddOption<TCheckboxGroupComponent>(this TCheckboxGroupComponent component, LocalCheckboxGroupOption option)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        if (component.Options.Add(option, out var list))
            component.Options = new(list);

        return component;
    }

    public static TCheckboxGroupComponent WithOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent component, IEnumerable<LocalCheckboxGroupOption> options)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        Guard.IsNotNull(options);

        if (component.Options.With(options, out var list))
            component.Options = new(list);

        return component;
    }

    public static TCheckboxGroupComponent WithOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent component, params LocalCheckboxGroupOption[] options)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
        => component.WithOptions((IEnumerable<LocalCheckboxGroupOption>) options);

    public static TCheckboxGroupComponent WithMinimumSelectedOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, int minimumSelectedOptions)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        checkboxGroupComponent.MinimumSelectedOptions = minimumSelectedOptions;
        return checkboxGroupComponent;
    }

    public static TCheckboxGroupComponent WithMaximumSelectedOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, int maximumSelectedOptions)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        checkboxGroupComponent.MaximumSelectedOptions = maximumSelectedOptions;
        return checkboxGroupComponent;
    }

    public static TCheckboxGroupComponent WithIsRequired<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, bool isRequired = true)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        checkboxGroupComponent.IsRequired = isRequired;
        return checkboxGroupComponent;
    }

    public static TCheckboxGroupComponent WithIsDisabled<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, bool isDisabled = true)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        checkboxGroupComponent.IsDisabled = isDisabled;
        return checkboxGroupComponent;
    }
}
