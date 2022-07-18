using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalRowComponentExtensions
{
    public static TComponent AddComponent<TComponent>(this TComponent row, LocalComponent component)
        where TComponent : LocalRowComponent
    {
        if (row.Components.Add(component, out var list))
            row.Components = new(list);

        return row;
    }

    public static TComponent WithComponents<TComponent>(this TComponent row, IEnumerable<LocalComponent> components)
        where TComponent : LocalRowComponent
    {
        Guard.IsNotNull(components);

        if (row.Components.With(components, out var list))
            row.Components = new(list);

        return row;
    }

    public static TComponent WithComponents<TComponent>(this TComponent row, params LocalComponent[] components)
        where TComponent : LocalRowComponent
    {
        return row.WithComponents(components as IEnumerable<LocalComponent>);
    }
}
