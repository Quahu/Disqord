using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientContainerComponent(IClient client, ContainerComponentJsonModel model)
    : TransientBaseComponent<ContainerComponentJsonModel>(client, model), IContainerComponent
{
    public Color? AccentColor => Model.AccentColor.GetValueOrDefault();

    [field: MaybeNull]
    public IReadOnlyList<IComponent> Components => field ??= Model.Components.ToReadOnlyList(Client,
        static (model, client) => TransientComponent.Create(client, model));

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
