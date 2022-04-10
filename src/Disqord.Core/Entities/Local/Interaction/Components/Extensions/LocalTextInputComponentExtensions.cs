namespace Disqord
{
    public static class LocalTextInputComponentExtensions
    {
        public static TComponent WithStyle<TComponent>(this TComponent component, TextInputComponentStyle style)
            where TComponent : LocalTextInputComponent
        {
            component.Style = style;
            return component;
        }

        public static TComponent WithLabel<TComponent>(this TComponent component, string label)
            where TComponent : LocalTextInputComponent
        {
            component.Label = label;
            return component;
        }

        public static TComponent WithMinimumInputLength<TComponent>(this TComponent component, int length)
            where TComponent : LocalTextInputComponent
        {
            component.MinimumInputLength = length;
            return component;
        }

        public static TComponent WithMaximumInputLength<TComponent>(this TComponent component, int length)
            where TComponent : LocalTextInputComponent
        {
            component.MaximumInputLength = length;
            return component;
        }

        public static TComponent WithIsRequired<TComponent>(this TComponent component, bool isRequired = true)
            where TComponent : LocalTextInputComponent
        {
            component.IsRequired = isRequired;
            return component;
        }

        public static TComponent WithPrefilledValue<TComponent>(this TComponent component, string value)
            where TComponent : LocalTextInputComponent
        {
            component.PrefilledValue = value;
            return component;
        }

        public static TComponent WithPlaceholder<TComponent>(this TComponent component, string placeholder)
            where TComponent : LocalTextInputComponent
        {
            component.Placeholder = placeholder;
            return component;
        }
    }
}
