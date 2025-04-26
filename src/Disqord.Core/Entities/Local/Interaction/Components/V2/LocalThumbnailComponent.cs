using Qommon;

namespace Disqord;

public class LocalThumbnailComponent : LocalComponent, ILocalComponentMediaItem, ILocalConstruct<LocalThumbnailComponent>
{
    /// <summary>
    ///     Gets or sets the media of this thumbnail.
    /// </summary>
    public Optional<LocalUnfurledMediaItem> Media { get; set; }

    /// <summary>
    ///     Gets or sets the description of this thumbnail.
    /// </summary>
    public Optional<string?> Description { get; set; }

    /// <summary>
    ///     Gets or sets whether this thumbnail is a spoiler.
    /// </summary>
    public Optional<bool> IsSpoiler { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalThumbnailComponent"/>.
    /// </summary>
    public LocalThumbnailComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalThumbnailComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalThumbnailComponent(LocalThumbnailComponent other)
    {
        Media = other.Media.Clone();
        Description = other.Description;
        IsSpoiler = other.IsSpoiler;
    }

    /// <inheritdoc/>
    public override LocalThumbnailComponent Clone()
    {
        return new(this);
    }

    public static LocalThumbnailComponent CreateFrom(IThumbnailComponent thumbnailComponent)
    {
        return new LocalThumbnailComponent
        {
            Id = thumbnailComponent.Id,
            Media = LocalUnfurledMediaItem.CreateFrom(thumbnailComponent.Media),
            Description = thumbnailComponent.Description,
            IsSpoiler = thumbnailComponent.IsSpoiler
        };
    }
}
