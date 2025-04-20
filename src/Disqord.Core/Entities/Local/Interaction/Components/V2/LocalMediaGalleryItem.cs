using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalMediaGalleryItem : ILocalConstruct<LocalMediaGalleryItem>, ILocalComponentMediaItem, IJsonConvertible<MediaGalleryItemJsonModel>
{
    /// <summary>
    ///     Gets or sets the media of this media gallery item.
    /// </summary>
    public Optional<LocalUnfurledMediaItem> Media { get; set; }

    /// <summary>
    ///     Gets or sets the description of this media gallery item.
    /// </summary>
    public Optional<string?> Description { get; set; }

    /// <summary>
    ///     Gets or sets whether this media gallery item is a spoiler.
    /// </summary>
    public Optional<bool> IsSpoiler { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMediaGalleryItem"/>.
    /// </summary>
    public LocalMediaGalleryItem()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMediaGalleryItem"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMediaGalleryItem(LocalMediaGalleryItem other)
    {
        Media = other.Media.Clone();
        Description = other.Description;
        IsSpoiler = other.IsSpoiler;
    }

    /// <inheritdoc/>
    public LocalMediaGalleryItem Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public MediaGalleryItemJsonModel ToModel()
    {
        OptionalGuard.HasValue(Media);

        return new MediaGalleryItemJsonModel
        {
            Media = Media.Value.ToModel(),
            Description = Description,
            Spoiler = IsSpoiler
        };
    }

    public static LocalMediaGalleryItem CreateFrom(IMediaGalleryItem mediaGalleryItem)
    {
        return new LocalMediaGalleryItem
        {
            Media = LocalUnfurledMediaItem.CreateFrom(mediaGalleryItem.Media),
            Description = mediaGalleryItem.Description,
            IsSpoiler = mediaGalleryItem.IsSpoiler
        };
    }
}
