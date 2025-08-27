using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientModalRowComponent(IClient client, ModalBaseComponentJsonModel model)
    : TransientModalComponent<ModalRowComponentJsonModel>(client, model), IModalRowComponent
{
    [field: MaybeNull]
    public IReadOnlyList<IModalComponent> Components => field ??= Model.Components.ToReadOnlyList(Client, static (model, client) => TransientModalComponent.Create(client, model));
}
