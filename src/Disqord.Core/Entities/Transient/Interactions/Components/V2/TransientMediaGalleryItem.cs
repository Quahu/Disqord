using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientMediaGalleryItem(MediaGalleryItemJsonModel model)
    : TransientEntity<MediaGalleryItemJsonModel>(model), IMediaGalleryItem
{
    [field: MaybeNull]
    public IUnfurledMediaItem Media => field ??= new TransientUnfurledMediaItem(Model.Media);

    public string? Description => Model.Description.GetValueOrDefault();

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
