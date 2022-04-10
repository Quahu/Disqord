using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalSelectionComponentExtensions
    {
        public static TComponent WithPlaceholder<TComponent>(this TComponent component, string placeholder)
            where TComponent : LocalSelectionComponent
        {
            component.Placeholder = placeholder;
            return component;
        }

        public static TComponent WithMinimumSelectedOptions<TComponent>(this TComponent component, int minimumSelectedOptions)
            where TComponent : LocalSelectionComponent
        {
            component.MinimumSelectedOptions = minimumSelectedOptions;
            return component;
        }

        public static TComponent WithMaximumSelectedOptions<TComponent>(this TComponent component, int maximumSelectedOptions)
            where TComponent : LocalSelectionComponent
        {
            component.MaximumSelectedOptions = maximumSelectedOptions;
            return component;
        }

        public static TComponent WithIsDisabled<TComponent>(this TComponent component, bool isDisabled = true)
            where TComponent : LocalSelectionComponent
        {
            component.IsDisabled = isDisabled;
            return component;
        }

        public static TComponent WithOptions<TComponent>(this TComponent rowComponent, params LocalSelectionComponentOption[] options)
            where TComponent : LocalSelectionComponent
            => rowComponent.WithOptions(options as IEnumerable<LocalSelectionComponentOption>);

        public static TComponent WithOptions<TComponent>(this TComponent rowComponent, IEnumerable<LocalSelectionComponentOption> options)
            where TComponent : LocalSelectionComponent
        {
            Guard.IsNotNull(options);

            rowComponent._options.Clear();
            rowComponent._options.AddRange(options);
            return rowComponent;
        }

        public static TComponent AddOption<TComponent>(this TComponent rowComponent, LocalSelectionComponentOption option)
            where TComponent : LocalSelectionComponent
        {
            rowComponent._options.Add(option);
            return rowComponent;
        }
    }
}
