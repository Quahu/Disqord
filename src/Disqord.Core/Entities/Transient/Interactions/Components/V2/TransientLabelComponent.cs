using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientLabelComponent(LabelComponentJsonModel model)
    : TransientBaseComponent<LabelComponentJsonModel>(model), ILabelComponent
{
    public string Label => model.Label;

    public string? Description => model.Description.GetValueOrDefault();

    [field: MaybeNull]
    public IComponent Component => field ??= TransientComponent.Create(Model.Component);
}
