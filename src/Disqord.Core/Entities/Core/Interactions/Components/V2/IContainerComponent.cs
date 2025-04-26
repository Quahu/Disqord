using System.Collections.Generic;

namespace Disqord;

public interface IContainerComponent : IComponent, ISpoilerableEntity
{
    Color? AccentColor { get; }

    IReadOnlyList<IComponent> Components { get; }
}
