using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalRadioGroupComponentExtensions
{
    public static TRadioGroupComponent AddOption<TRadioGroupComponent>(this TRadioGroupComponent component, string label, string value, string? description = null, bool isDefault = false)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        var option = new LocalRadioGroupOption(label, value)
        {
            IsDefault = isDefault
        };

        if (description != null)
            option.Description = description;

        return component.AddOption(option);
    }

    public static TRadioGroupComponent AddOption<TRadioGroupComponent>(this TRadioGroupComponent component, LocalRadioGroupOption option)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        if (component.Options.Add(option, out var list))
            component.Options = new(list);

        return component;
    }

    public static TRadioGroupComponent WithOptions<TRadioGroupComponent>(this TRadioGroupComponent component, IEnumerable<LocalRadioGroupOption> options)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        Guard.IsNotNull(options);

        if (component.Options.With(options, out var list))
            component.Options = new(list);

        return component;
    }

    public static TRadioGroupComponent WithOptions<TRadioGroupComponent>(this TRadioGroupComponent component, params LocalRadioGroupOption[] options)
        where TRadioGroupComponent : LocalRadioGroupComponent
        => component.WithOptions((IEnumerable<LocalRadioGroupOption>) options);

    public static TRadioGroupComponent WithIsRequired<TRadioGroupComponent>(this TRadioGroupComponent radioGroupComponent, bool isRequired = true)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        radioGroupComponent.IsRequired = isRequired;
        return radioGroupComponent;
    }

    public static TRadioGroupComponent WithIsDisabled<TRadioGroupComponent>(this TRadioGroupComponent radioGroupComponent, bool isDisabled = true)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        radioGroupComponent.IsDisabled = isDisabled;
        return radioGroupComponent;
    }
}
