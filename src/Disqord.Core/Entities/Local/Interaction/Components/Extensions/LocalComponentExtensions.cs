namespace Disqord;

public static class LocalComponentExtensions
{
    public static TComponent WithId<TComponent>(this TComponent component, int id)
        where TComponent : LocalComponent
    {
        component.Id = id;
        return component;
    }
}
