using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalUnfurledMediaItem : ILocalConstruct<LocalUnfurledMediaItem>, IJsonConvertible<UnfurledMediaItemJsonModel>
{
    /// <summary>
    ///     Gets or sets the URL of this unfurled media item.
    /// </summary>
    public Optional<string> Url { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalUnfurledMediaItem"/>.
    /// </summary>
    public LocalUnfurledMediaItem()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalUnfurledMediaItem"/> with a URL.
    /// </summary>
    public LocalUnfurledMediaItem(string url)
    {
        Url = url;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalUnfurledMediaItem"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalUnfurledMediaItem(LocalUnfurledMediaItem other)
    {
        Url = other.Url;
    }

    /// <inheritdoc/>
    public virtual LocalUnfurledMediaItem Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual UnfurledMediaItemJsonModel ToModel()
    {
        OptionalGuard.HasValue(Url);

        return new UnfurledMediaItemJsonModel
        {
            Url = Url.Value
        };
    }

    public static LocalUnfurledMediaItem CreateFrom(IUnfurledMediaItem unfurledMediaItem)
    {
        return new LocalUnfurledMediaItem
        {
            Url = unfurledMediaItem.Url
        };
    }
}
