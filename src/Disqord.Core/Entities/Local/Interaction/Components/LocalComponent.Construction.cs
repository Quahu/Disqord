using System.Collections.Generic;

namespace Disqord;

public abstract partial class LocalComponent
{
    public static LocalRowComponent Row(params LocalComponent[] components)
    {
        return new LocalRowComponent
        {
            Components = components
        };
    }

    public static LocalButtonComponent Button(string customId, string label)
    {
        return new LocalButtonComponent
        {
            CustomId = customId,
            Label = label
        };
    }

    public static LocalButtonComponent Button(string customId, LocalEmoji emoji)
    {
        return new LocalButtonComponent
        {
            CustomId = customId,
            Emoji = emoji
        };
    }

    public static LocalLinkButtonComponent LinkButton(string url, string label)
    {
        return new LocalLinkButtonComponent
        {
            Url = url,
            Label = label
        };
    }

    public static LocalLinkButtonComponent LinkButton(string url, LocalEmoji emoji)
    {
        return new LocalLinkButtonComponent
        {
            Url = url,
            Emoji = emoji
        };
    }

    public static LocalSelectionComponent Selection(string customId, params LocalSelectionComponentOption[] options)
    {
        return new LocalSelectionComponent
        {
            CustomId = customId,
            Options = options
        };
    }

    public static LocalTextInputComponent TextInput(string customId, string label, TextInputComponentStyle style)
    {
        return new LocalTextInputComponent
        {
            Style = style,
            CustomId = customId,
            Label = label
        };
    }

    public static LocalSectionComponent Section(LocalComponent accessory, params IEnumerable<LocalComponent> components)
    {
        return new LocalSectionComponent()
            .WithAccessory(accessory)
            .WithComponents(components);
    }

    public static LocalTextDisplayComponent TextDisplay(string content)
    {
        return new LocalTextDisplayComponent()
            .WithContent(content);
    }

    public static LocalThumbnailComponent Thumbnail(string url, string? description = null, bool isSpoiler = false)
    {
        return Thumbnail(new LocalUnfurledMediaItem(url));
    }

    public static LocalThumbnailComponent Thumbnail(LocalUnfurledMediaItem media, string? description = null, bool isSpoiler = false)
    {
        return new LocalThumbnailComponent()
            .WithMedia(media)
            .WithDescription(description)
            .WithIsSpoiler(isSpoiler);
    }

    public static LocalMediaGalleryComponent MediaGallery(params IEnumerable<LocalMediaGalleryItem> items)
    {
        return new LocalMediaGalleryComponent()
            .WithItems(items);
    }

    public static LocalFileComponent File(string url)
    {
        return File(new LocalUnfurledMediaItem(url));
    }

    public static LocalFileComponent File(LocalUnfurledMediaItem file)
    {
        return new LocalFileComponent()
            .WithFile(file);
    }

    public static LocalSeparatorComponent Separator(bool isDivider = true, SeparatorComponentSpacingSize spacingSize = SeparatorComponentSpacingSize.Small)
    {
        return new LocalSeparatorComponent()
            .WithIsDivider(isDivider)
            .WithSpacingSize(spacingSize);
    }

    public static LocalContainerComponent Container(Color accentColor, params IEnumerable<LocalComponent> components)
    {
        return Container(components)
            .WithAccentColor(accentColor);
    }

    public static LocalContainerComponent Container(params IEnumerable<LocalComponent> components)
    {
        return new LocalContainerComponent()
            .WithComponents(components);
    }
}
