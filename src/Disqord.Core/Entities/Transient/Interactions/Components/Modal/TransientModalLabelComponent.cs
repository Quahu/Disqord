using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientModalLabelComponent(IClient client, ModalLabelComponentJsonModel model)
    : TransientModalComponent<ModalLabelComponentJsonModel>(client, model), IModalLabelComponent
{
    [field: MaybeNull]
    public IModalComponent Component => field ??= TransientModalComponent.Create(Client, Model.Component);
}
