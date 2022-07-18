using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalTextInputComponentExtensions
{
    public static TComponent WithStyle<TComponent>(this TComponent textInput, TextInputComponentStyle style)
        where TComponent : LocalTextInputComponent
    {
        textInput.Style = style;
        return textInput;
    }

    public static TComponent WithLabel<TComponent>(this TComponent textInput, string label)
        where TComponent : LocalTextInputComponent
    {
        textInput.Label = label;
        return textInput;
    }

    public static TComponent WithMinimumInputLength<TComponent>(this TComponent textInput, int length)
        where TComponent : LocalTextInputComponent
    {
        textInput.MinimumInputLength = length;
        return textInput;
    }

    public static TComponent WithMaximumInputLength<TComponent>(this TComponent textInput, int length)
        where TComponent : LocalTextInputComponent
    {
        textInput.MaximumInputLength = length;
        return textInput;
    }

    public static TComponent WithIsRequired<TComponent>(this TComponent textInput, bool isRequired = true)
        where TComponent : LocalTextInputComponent
    {
        textInput.IsRequired = isRequired;
        return textInput;
    }

    public static TComponent WithPrefilledValue<TComponent>(this TComponent textInput, string value)
        where TComponent : LocalTextInputComponent
    {
        textInput.PrefilledValue = value;
        return textInput;
    }

    public static TComponent WithPlaceholder<TComponent>(this TComponent textInput, string placeholder)
        where TComponent : LocalTextInputComponent
    {
        textInput.Placeholder = placeholder;
        return textInput;
    }
}
