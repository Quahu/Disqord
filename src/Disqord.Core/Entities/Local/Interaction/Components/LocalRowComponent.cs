using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalRowComponent : LocalComponent, ILocalConstruct<LocalRowComponent>
{
    public Optional<IList<LocalComponent>> Components { get; set; }

    public LocalRowComponent()
    { }

    protected LocalRowComponent(LocalRowComponent other)
    {
        Components = other.Components.DeepClone();
    }

    public override LocalRowComponent Clone()
    {
        return new(this);
    }
}
