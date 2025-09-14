using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IComponent"/>
public class TransientComponent(ComponentJsonModel model)
    : TransientBaseComponent<ComponentJsonModel>(model)
{
    public static IComponent Create(BaseComponentJsonModel model)
    {
        return model.Type switch
        {
            ComponentType.Row => new TransientRowComponent(Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.Button => new TransientButtonComponent(Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.StringSelection or (>= ComponentType.UserSelection and <= ComponentType.ChannelSelection) => new TransientSelectionComponent(Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.TextInput => new TransientTextInputComponent(Guard.IsOfType<ComponentJsonModel>(model)),
            ComponentType.Section => new TransientSectionComponent(Guard.IsOfType<SectionComponentJsonModel>(model)),
            ComponentType.TextDisplay => new TransientTextDisplayComponent(Guard.IsOfType<TextDisplayComponentJsonModel>(model)),
            ComponentType.Thumbnail => new TransientThumbnailComponent(Guard.IsOfType<ThumbnailComponentJsonModel>(model)),
            ComponentType.MediaGallery => new TransientMediaGalleryComponent(Guard.IsOfType<MediaGalleryComponentJsonModel>(model)),
            ComponentType.File => new TransientFileComponent(Guard.IsOfType<FileComponentJsonModel>(model)),
            ComponentType.Separator => new TransientSeparatorComponent(Guard.IsOfType<SeparatorComponentJsonModel>(model)),
            ComponentType.Container => new TransientContainerComponent(Guard.IsOfType<ContainerComponentJsonModel>(model)),
            ComponentType.Label => new TransientLabelComponent(Guard.IsOfType<LabelComponentJsonModel>(model)),
            _ => new TransientBaseComponent<BaseComponentJsonModel>(model)
        };
    }
}
