namespace Disqord;

public static class LocalContainerComponentExtensions
{
    public static TComponent WithAccentColor<TComponent>(this TComponent component, Color? accentColor)
        where TComponent : LocalContainerComponent
    {
        component.AccentColor = accentColor;
        return component;
    }
}
