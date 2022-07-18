using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalButtonComponentBaseExtensions
{
    public static TComponent WithLabel<TComponent>(this TComponent component, string label)
        where TComponent : LocalButtonComponentBase
    {
        component.Label = label;
        return component;
    }

    public static TComponent WithEmoji<TComponent>(this TComponent component, LocalEmoji emoji)
        where TComponent : LocalButtonComponentBase
    {
        component.Emoji = emoji;
        return component;
    }

    public static TComponent WithIsDisabled<TComponent>(this TComponent component, bool isDisabled = true)
        where TComponent : LocalButtonComponentBase
    {
        component.IsDisabled = isDisabled;
        return component;
    }
}
