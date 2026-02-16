using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalCheckboxGroupComponentExtensions
{
    public static TCheckboxGroupComponent WithCustomId<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, string customId)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        checkboxGroupComponent.CustomId = customId;
        return checkboxGroupComponent;
    }

    public static TCheckboxGroupComponent WithOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, IEnumerable<LocalCheckboxGroupOption> options)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
    {
        if (checkboxGroupComponent.Options.HasValue)
        {
            checkboxGroupComponent.Options.Value.Clear();
            foreach (var option in options)
                checkboxGroupComponent.Options.Value.Add(option);
        }
        else
        {
            checkboxGroupComponent.Options = new List<LocalCheckboxGroupOption>(options);
        }

        return checkboxGroupComponent;
    }

    public static TCheckboxGroupComponent WithOptions<TCheckboxGroupComponent>(this TCheckboxGroupComponent checkboxGroupComponent, params LocalCheckboxGroupOption[] options)
        where TCheckboxGroupComponent : LocalCheckboxGroupComponent
        => checkboxGroupComponent.WithOptions((IEnumerable<LocalCheckboxGroupOption>) options);

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
