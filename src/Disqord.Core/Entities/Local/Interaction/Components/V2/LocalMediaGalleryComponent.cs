using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord;

public class LocalMediaGalleryComponent : LocalComponent, ILocalConstruct<LocalMediaGalleryComponent>
{
    /// <summary>
    ///     Gets or sets the media items of this gallery.
    /// </summary>
    public Optional<IList<LocalMediaGalleryItem>> Items { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMediaGalleryComponent"/>.
    /// </summary>
    public LocalMediaGalleryComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalMediaGalleryComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalMediaGalleryComponent(LocalMediaGalleryComponent other)
    {
        Items = other.Items.DeepClone();
    }

    /// <inheritdoc/>
    public override LocalMediaGalleryComponent Clone()
    {
        return new(this);
    }

    public static LocalMediaGalleryComponent CreateFrom(IMediaGalleryComponent mediaGalleryComponent)
    {
        return new LocalMediaGalleryComponent
        {
            Id = mediaGalleryComponent.Id,
            Items = mediaGalleryComponent.Items.Select(LocalMediaGalleryItem.CreateFrom).ToArray()
        };
    }
}
