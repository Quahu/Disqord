namespace Disqord;

public static class LocalSeparatorComponentExtensions
{
    public static TSeparatorComponent WithIsDivider<TSeparatorComponent>(this TSeparatorComponent component, bool isDivider = true)
        where TSeparatorComponent : LocalSeparatorComponent
    {
        component.IsDivider = isDivider;
        return component;
    }

    public static TSeparatorComponent WithSpacingSize<TSeparatorComponent>(this TSeparatorComponent component, SeparatorComponentSpacingSize spacingSize)
        where TSeparatorComponent : LocalSeparatorComponent
    {
        component.SpacingSize = spacingSize;
        return component;
    }
}
