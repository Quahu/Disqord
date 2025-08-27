namespace Disqord;

public static class LocalLabelComponentExtensions
{
    public static TLabelComponent WithLabel<TLabelComponent>(this TLabelComponent labelComponent, string label)
        where TLabelComponent : LocalLabelComponent
    {
        labelComponent.Label = label;
        return labelComponent;
    }

    public static TLabelComponent WithDescription<TLabelComponent>(this TLabelComponent labelComponent, string description)
        where TLabelComponent : LocalLabelComponent
    {
        labelComponent.Description = description;
        return labelComponent;
    }

    public static TLabelComponent WithComponent<TLabelComponent>(this TLabelComponent labelComponent, LocalComponent component)
        where TLabelComponent : LocalLabelComponent
    {
        labelComponent.Component = component;
        return labelComponent;
    }
}
