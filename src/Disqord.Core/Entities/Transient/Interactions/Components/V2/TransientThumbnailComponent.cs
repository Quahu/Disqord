using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientThumbnailComponent(ThumbnailComponentJsonModel model)
    : TransientBaseComponent<ThumbnailComponentJsonModel>(model), IThumbnailComponent
{
    [field: MaybeNull]
    public IUnfurledMediaItem Media => field ??= TransientUnfurledMediaItem.Create(Model.Media);

    public string? Description => Model.Description.GetValueOrDefault();

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
