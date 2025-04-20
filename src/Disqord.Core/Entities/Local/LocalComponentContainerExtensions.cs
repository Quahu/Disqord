using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalComponentContainerExtensions
{
    public static TComponentContainer AddComponent<TComponentContainer>(this TComponentContainer container, LocalComponent component)
        where TComponentContainer : ILocalComponentContainer
    {
        if (container.Components.Add(component, out var list))
            container.Components = new(list);

        return container;
    }

    public static TComponentContainer WithComponents<TComponentContainer>(this TComponentContainer container, IEnumerable<LocalComponent> components)
        where TComponentContainer : ILocalComponentContainer
    {
        Guard.IsNotNull(components);

        if (container.Components.With(components, out var list))
            container.Components = new(list);

        return container;
    }

    public static TComponentContainer WithComponents<TComponentContainer>(this TComponentContainer container, params LocalComponent[] components)
        where TComponentContainer : ILocalComponentContainer
    {
        return container.WithComponents(components as IEnumerable<LocalComponent>);
    }
}
