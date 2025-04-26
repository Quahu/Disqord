namespace Disqord;

public static class LocalComponentMediaItemExtensions
{
    public static TMediaItem WithMedia<TMediaItem>(this TMediaItem mediaItem, string url)
        where TMediaItem : ILocalComponentMediaItem
    {
        return mediaItem.WithMedia(new LocalUnfurledMediaItem(url));
    }

    public static TMediaItem WithMedia<TMediaItem>(this TMediaItem mediaItem, LocalUnfurledMediaItem media)
        where TMediaItem : ILocalComponentMediaItem
    {
        mediaItem.Media = media;
        return mediaItem;
    }

    public static TMediaItem WithDescription<TMediaItem>(this TMediaItem mediaItem, string? description)
        where TMediaItem : ILocalComponentMediaItem
    {
        mediaItem.Description = description;
        return mediaItem;
    }
}
