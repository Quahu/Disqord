using System;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public abstract partial class LocalComponent : ILocalConstruct<LocalComponent>, IJsonConvertible<BaseComponentJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of this component.
    ///     If not set, Discord will set it based on an incrementing value.
    /// </summary>
    public Optional<int> Id { get; set; }

    protected LocalComponent()
    { }

    protected LocalComponent(LocalComponent other)
    {
        Id = other.Id;
    }

    /// <inheritdoc/>
    public abstract LocalComponent Clone();

    /// <inheritdoc/>
    public virtual BaseComponentJsonModel ToModel()
    {
        if (!IsComponentV2())
        {
            return CreateComponentJsonModel();
        }

        BaseComponentJsonModel model;
        switch (this)
        {
            case LocalSectionComponent section:
                model = new SectionComponentJsonModel
                {
                    Components = Optional.ConvertOrDefault(section.Components, static components => components.Select(static component => component.ToModel()).ToArray()) ?? [],
                    Accessory = Optional.ConvertOrDefault(section.Accessory, static accessory => accessory.ToModel())!
                };

                break;
            case LocalTextDisplayComponent textDisplay:
                OptionalGuard.HasValue(textDisplay.Content);

                model = new TextDisplayComponentJsonModel
                {
                    Content = textDisplay.Content.Value
                };

                break;
            case LocalThumbnailComponent thumbnail:
                OptionalGuard.HasValue(thumbnail.Media);

                model = new ThumbnailComponentJsonModel
                {
                    Media = thumbnail.Media.Value.ToModel(),
                    Description = thumbnail.Description,
                    Spoiler = thumbnail.IsSpoiler
                };

                break;
            case LocalMediaGalleryComponent mediaGallery:
                model = new MediaGalleryComponentJsonModel
                {
                    Items = Optional.ConvertOrDefault(mediaGallery.Items, static items => items.Select(static item => item.ToModel()).ToArray()) ?? []
                };

                break;
            case LocalFileComponent file:
                OptionalGuard.HasValue(file.File);

                model = new FileComponentJsonModel
                {
                    File = file.File.Value.ToModel(),
                    Spoiler = file.IsSpoiler
                };

                break;
            case LocalSeparatorComponent separator:
                model = new SeparatorComponentJsonModel
                {
                    Divider = separator.IsDivider,
                    Spacing = separator.SpacingSize
                };

                break;
            case LocalContainerComponent container:
                model = new ContainerComponentJsonModel
                {
                    Components = Optional.ConvertOrDefault(container.Components, static components => components.Select(static component => component.ToModel()).ToArray()) ?? [],
                    AccentColor = Optional.Convert(container.AccentColor, static color => color?.RawValue),
                    Spoiler = container.IsSpoiler
                };

                break;
            default:
                throw new InvalidOperationException("Unknown local component type.");
        }

        model.Id = Id;

        return model;
    }

    private ComponentJsonModel CreateComponentJsonModel()
    {
        // TODO: maybe split this via inheritance
        var model = new ComponentJsonModel();

        if (this is ILocalCustomIdentifiableEntity customIdentifiableEntity)
            model.CustomId = customIdentifiableEntity.CustomId;

        if (this is LocalRowComponent rowComponent)
        {
            model.Type = ComponentType.Row;
            model.Components = Optional.Convert(rowComponent.Components, components => components.Select(component => component.ToModel()).ToArray());
        }
        else if (this is LocalButtonComponentBase buttonComponentBase)
        {
            model.Type = ComponentType.Button;
            model.Label = buttonComponentBase.Label;
            model.Emoji = Optional.Convert(buttonComponentBase.Emoji, emoji => emoji.ToModel());
            model.Disabled = buttonComponentBase.IsDisabled;

            if (buttonComponentBase is LocalButtonComponent buttonComponent)
            {
                model.Style = Optional.Convert(buttonComponent.Style, style => (byte) style);
            }
            else if (buttonComponentBase is LocalLinkButtonComponent linkButtonComponent)
            {
                model.Style = (byte) ButtonComponentStyle.Link;
                model.Url = linkButtonComponent.Url;
            }
            else
            {
                throw new InvalidOperationException("Unknown local button component type.");
            }
        }
        else if (this is LocalSelectionComponent selectionComponent)
        {
            model.Type = (ComponentType) selectionComponent.Type;
            model.ChannelTypes = Optional.Convert(selectionComponent.ChannelTypes, channelTypes => channelTypes?.ToArray())!;
            model.Placeholder = selectionComponent.Placeholder;
            model.DefaultValues = Optional.Convert(selectionComponent.DefaultValues, defaultValues => defaultValues.Select(defaultValue => defaultValue.ToModel()).ToArray());
            model.MinValues = selectionComponent.MinimumSelectedOptions;
            model.MaxValues = selectionComponent.MaximumSelectedOptions;
            model.Disabled = selectionComponent.IsDisabled;
            model.Options = Optional.Convert(selectionComponent.Options, options => options.Select(option => option.ToModel()).ToArray());
        }
        else if (this is LocalTextInputComponent textInputComponent)
        {
            model.Type = ComponentType.TextInput;
            model.Style = Optional.Convert(textInputComponent.Style, style => (byte) style);
            model.CustomId = textInputComponent.CustomId;
            model.Label = textInputComponent.Label;
            model.MinLength = textInputComponent.MinimumInputLength;
            model.MaxLength = textInputComponent.MaximumInputLength;
            model.Required = textInputComponent.IsRequired;
            model.Value = textInputComponent.PrefilledValue;
            model.Placeholder = textInputComponent.Placeholder;
        }
        else
        {
            throw new InvalidOperationException("Unknown local component type.");
        }

        return model;
    }

    /// <summary>
    ///     Converts the specified component to a <see cref="LocalComponent"/>.
    /// </summary>
    /// <param name="component"> The component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalComponent"/>.
    /// </returns>
    public static LocalComponent CreateFrom(IComponent component)
    {
        return component switch
        {
            IRowComponent rowComponent => LocalRowComponent.CreateFrom(rowComponent),
            IButtonComponent buttonComponent => LocalButtonComponentBase.CreateFrom(buttonComponent),
            ISelectionComponent selectionComponent => LocalSelectionComponent.CreateFrom(selectionComponent),
            ITextInputComponent textInputComponent => LocalTextInputComponent.CreateFrom(textInputComponent),
            ISectionComponent sectionComponent => LocalSectionComponent.CreateFrom(sectionComponent),
            ITextDisplayComponent textDisplayComponent => LocalTextDisplayComponent.CreateFrom(textDisplayComponent),
            IThumbnailComponent thumbnailComponent => LocalThumbnailComponent.CreateFrom(thumbnailComponent),
            IMediaGalleryComponent mediaGalleryComponent => LocalMediaGalleryComponent.CreateFrom(mediaGalleryComponent),
            IFileComponent => Throw.ArgumentException<LocalComponent>(
                "Cannot convert file components to local file components as they do not support arbitrary external urls."
                + "You must use the `attachment://` reference system instead."),
            ISeparatorComponent separatorComponent => LocalSeparatorComponent.CreateFrom(separatorComponent),
            IContainerComponent containerComponent => LocalContainerComponent.CreateFrom(containerComponent),
            _ => Throw.ArgumentException<LocalComponent>("Unsupported component type.", nameof(component))
        };
    }

    internal bool IsComponentV2()
    {
        return this is not (LocalRowComponent or LocalButtonComponentBase or LocalSelectionComponent or LocalTextInputComponent);
    }
}
