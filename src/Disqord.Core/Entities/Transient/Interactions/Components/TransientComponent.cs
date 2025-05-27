using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IComponent"/>
public class TransientComponent : TransientBaseComponent<ComponentJsonModel>
{
    public TransientComponent(IClient client, ComponentJsonModel model)
        : base(client, model)
    { }

    public static IComponent Create(IClient client, BaseComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientRowComponent(client, (ComponentJsonModel) model),
            ComponentType.Button => new TransientButtonComponent(client, (ComponentJsonModel) model),
            ComponentType.StringSelection => new TransientSelectionComponent(client, (ComponentJsonModel) model),
            ComponentType.TextInput => new TransientTextInputComponent(client, (ComponentJsonModel) model),
            >= ComponentType.UserSelection and <= ComponentType.ChannelSelection => new TransientSelectionComponent(client, (ComponentJsonModel) model),
            ComponentType.Section => new TransientSectionComponent(client, (SectionComponentJsonModel) model),
            ComponentType.TextDisplay => new TransientTextDisplayComponent(client, (TextDisplayComponentJsonModel) model),
            ComponentType.Thumbnail => new TransientThumbnailComponent(client, (ThumbnailComponentJsonModel) model),
            ComponentType.MediaGallery => new TransientMediaGalleryComponent(client, (MediaGalleryComponentJsonModel) model),
            ComponentType.File => new TransientFileComponent(client, (FileComponentJsonModel) model),
            ComponentType.Separator => new TransientSeparatorComponent(client, (SeparatorComponentJsonModel) model),
            ComponentType.Container => new TransientContainerComponent(client, (ContainerComponentJsonModel) model),
            _ => new TransientBaseComponent<BaseComponentJsonModel>(client, model)
        };
    }
}
