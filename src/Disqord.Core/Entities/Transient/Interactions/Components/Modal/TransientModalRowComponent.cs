using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientModalRowComponent(ModalBaseComponentJsonModel model)
    : TransientModalComponent<ModalRowComponentJsonModel>(model), IModalRowComponent
{
    [field: MaybeNull]
    public IReadOnlyList<IModalComponent> Components => field ??= Model.Components.ToReadOnlyList(TransientModalComponent.Create);
}
