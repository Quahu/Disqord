using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalLinkButtonComponentExtensions
{
    public static TComponent WithUrl<TComponent>(this TComponent component, string url)
        where TComponent : LocalLinkButtonComponent
    {
        component.Url = url;
        return component;
    }
}
