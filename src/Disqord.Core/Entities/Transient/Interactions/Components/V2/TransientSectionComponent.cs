using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSectionComponent(IClient client, SectionComponentJsonModel model)
    : TransientBaseComponent<SectionComponentJsonModel>(client, model), ISectionComponent
{
    [field: MaybeNull]
    public IReadOnlyList<IComponent> Components => field ??= Model.Components.ToReadOnlyList(Client,
        static (model, client) => TransientComponent.Create(client, model));

    [field: MaybeNull]
    public IComponent Accessory => field ??= TransientComponent.Create(Client, Model.Accessory);
}
