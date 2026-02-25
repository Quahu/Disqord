using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSectionComponent(SectionComponentJsonModel model)
    : TransientBaseComponent<SectionComponentJsonModel>(model), ISectionComponent
{
    [field: MaybeNull]
    public IReadOnlyList<IComponent> Components => field ??= Model.Components.ToReadOnlyList(TransientComponent.Create);

    [field: MaybeNull]
    public IComponent Accessory => field ??= TransientComponent.Create(Model.Accessory);
}
