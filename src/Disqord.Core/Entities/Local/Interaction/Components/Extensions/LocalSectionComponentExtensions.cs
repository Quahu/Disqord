namespace Disqord;

public static class LocalSectionComponentExtensions
{
    public static TSectionComponent WithAccessory<TSectionComponent>(this TSectionComponent component, LocalComponent accessory)
        where TSectionComponent : LocalSectionComponent
    {
        component.Accessory = accessory;
        return component;
    }
}
