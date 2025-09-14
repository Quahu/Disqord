using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientContainerComponent(ContainerComponentJsonModel model)
    : TransientBaseComponent<ContainerComponentJsonModel>(model), IContainerComponent
{
    public Color? AccentColor => Model.AccentColor.GetValueOrDefault();

    [field: MaybeNull]
    public IReadOnlyList<IComponent> Components => field ??= Model.Components.ToReadOnlyList(TransientComponent.Create);

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
