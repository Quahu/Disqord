using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalRowComponentExtensions
    {
        public static TComponent WithComponents<TComponent>(this TComponent rowComponent, params LocalNestedComponent[] components)
            where TComponent : LocalRowComponent
            => rowComponent.WithComponents(components as IEnumerable<LocalNestedComponent>);

        public static TComponent WithComponents<TComponent>(this TComponent rowComponent, IEnumerable<LocalNestedComponent> components)
            where TComponent : LocalRowComponent
        {
            Guard.IsNotNull(components);

            rowComponent._components.Clear();
            rowComponent._components.AddRange(components);
            return rowComponent;
        }

        public static TComponent AddComponent<TComponent>(this TComponent rowComponent, LocalNestedComponent component)
            where TComponent : LocalRowComponent
        {
            rowComponent._components.Add(component);
            return rowComponent;
        }
    }
}
