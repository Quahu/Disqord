using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientModalLabelComponent(ModalLabelComponentJsonModel model)
    : TransientModalComponent<ModalLabelComponentJsonModel>(model), IModalLabelComponent
{
    [field: MaybeNull]
    public IModalComponent Component => field ??= TransientModalComponent.Create(Model.Component);
}
