using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IComponent"/>
public class TransientComponent : TransientClientEntity<ComponentJsonModel>, IComponent
{
    /// <inheritdoc/>
    public ComponentType Type => Model.Type;

    public TransientComponent(IClient client, ComponentJsonModel model)
        : base(client, model)
    { }

    public static IComponent Create(IClient client, ComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientRowComponent(client, model),
            ComponentType.Button => new TransientButtonComponent(client, model),
            ComponentType.Selection => new TransientSelectionComponent(client, model),
            ComponentType.TextInput => new TransientTextInputComponent(client, model),
            _ => new TransientComponent(client, model)
        };
    }
}
