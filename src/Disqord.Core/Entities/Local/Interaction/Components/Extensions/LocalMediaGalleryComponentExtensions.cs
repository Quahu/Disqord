using System.Collections.Generic;
using Qommon;

namespace Disqord;

public static class LocalMediaGalleryComponentExtensions
{
    public static TMediaGalleryComponent AddItem<TMediaGalleryComponent>(this TMediaGalleryComponent component, string url, string? description = null, bool isSpoiler = false)
        where TMediaGalleryComponent : LocalMediaGalleryComponent
    {
        return component.AddItem(new LocalUnfurledMediaItem(url), description, isSpoiler);
    }

    public static TMediaGalleryComponent AddItem<TMediaGalleryComponent>(this TMediaGalleryComponent component, LocalUnfurledMediaItem media, string? description = null, bool isSpoiler = false)
        where TMediaGalleryComponent : LocalMediaGalleryComponent
    {
        return component.AddItem(new LocalMediaGalleryItem()
            .WithMedia(media)
            .WithDescription(description)
            .WithIsSpoiler(isSpoiler));
    }

    public static TMediaGalleryComponent AddItem<TMediaGalleryComponent>(this TMediaGalleryComponent component, LocalMediaGalleryItem item)
        where TMediaGalleryComponent : LocalMediaGalleryComponent
    {
        if (component.Items.Add(item, out var list))
            component.Items = new(list);

        return component;
    }

    public static TMediaGalleryComponent WithItems<TMediaGalleryComponent>(this TMediaGalleryComponent component, IEnumerable<LocalMediaGalleryItem> items)
        where TMediaGalleryComponent : LocalMediaGalleryComponent
    {
        Guard.IsNotNull(items);

        if (component.Items.With(items, out var list))
            component.Items = new(list);

        return component;
    }
}
