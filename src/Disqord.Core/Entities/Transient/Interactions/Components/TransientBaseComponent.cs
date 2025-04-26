using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IComponent"/>
public class TransientBaseComponent<TComponentModel>(IClient client, BaseComponentJsonModel model)
    : TransientClientEntity<BaseComponentJsonModel>(client, model), IComponent
    where TComponentModel : BaseComponentJsonModel
{
    public int Id => Model.Id.Value;

    public ComponentType Type => Model.Type;

    public new TComponentModel Model => (TComponentModel) base.Model;
}
