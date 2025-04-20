using System.Collections.Generic;

namespace Disqord;

public interface ISectionComponent : IComponent
{
    IReadOnlyList<IComponent> Components { get; }

    IComponent Accessory { get; }
}
