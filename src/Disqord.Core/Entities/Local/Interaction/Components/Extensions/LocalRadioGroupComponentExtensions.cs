using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalRadioGroupComponentExtensions
{
    public static TRadioGroupComponent WithCustomId<TRadioGroupComponent>(this TRadioGroupComponent radioGroupComponent, string customId)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        radioGroupComponent.CustomId = customId;
        return radioGroupComponent;
    }

    public static TRadioGroupComponent WithOptions<TRadioGroupComponent>(this TRadioGroupComponent radioGroupComponent, IEnumerable<LocalRadioGroupOption> options)
        where TRadioGroupComponent : LocalRadioGroupComponent
    {
        if (radioGroupComponent.Options.HasValue)
        {
            radioGroupComponent.Options.Value.Clear();
            foreach (var option in options)
                radioGroupComponent.Options.Value.Add(option);
        }
        else
        {
            radioGroupComponent.Options = new List<LocalRadioGroupOption>(options);
        }

        return radioGroupComponent;
    }

    public static TRadioGroupComponent WithOptions<TRadioGroupComponent>(this TRadioGroupComponent radioGroupComponent, params LocalRadioGroupOption[] options)
        where TRadioGroupComponent : LocalRadioGroupComponent
        => radioGroupComponent.WithOptions((IEnumerable<LocalRadioGroupOption>) options);

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
