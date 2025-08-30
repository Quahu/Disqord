using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientLabelComponent(IClient client, LabelComponentJsonModel model)
    : TransientBaseComponent<LabelComponentJsonModel>(client, model), ILabelComponent
{
    public string Label => model.Label;

    public string? Description => model.Description.GetValueOrDefault();

    [field: MaybeNull]
    public IComponent Component => field ??= TransientComponent.Create(Client, Model.Component);
}
