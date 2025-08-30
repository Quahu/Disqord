using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IComponent"/>
public class TransientComponent(IClient client, ComponentJsonModel model)
    : TransientBaseComponent<ComponentJsonModel>(client, model), IComponent
{
    public static IComponent Create(IClient client, BaseComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientRowComponent(client, Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.Button => new TransientButtonComponent(client, Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.StringSelection or (>= ComponentType.UserSelection and <= ComponentType.ChannelSelection) => new TransientSelectionComponent(client, Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.TextInput => new TransientTextInputComponent(client, Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.Section => new TransientSectionComponent(client, Guard.IsOfType<SectionComponentJsonModel>(model)),
            ComponentType.TextDisplay => new TransientTextDisplayComponent(client, Guard.IsOfType<TextDisplayComponentJsonModel>(model)),
            ComponentType.Thumbnail => new TransientThumbnailComponent(client, Guard.IsOfType<ThumbnailComponentJsonModel>(model)),
            ComponentType.MediaGallery => new TransientMediaGalleryComponent(client, Guard.IsOfType<MediaGalleryComponentJsonModel>(model)),
            ComponentType.File => new TransientFileComponent(client, Guard.IsOfType<FileComponentJsonModel>(model)),
            ComponentType.Separator => new TransientSeparatorComponent(client, Guard.IsOfType<SeparatorComponentJsonModel>(model)),
            ComponentType.Container => new TransientContainerComponent(client, Guard.IsOfType<ContainerComponentJsonModel>(model)),
            ComponentType.Label => new TransientLabelComponent(client, Guard.IsOfType<LabelComponentJsonModel>(model)),
            _ => new TransientBaseComponent<BaseComponentJsonModel>(client, model)
        };
    }
}
