using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientThumbnailComponent(IClient client, ThumbnailComponentJsonModel model)
    : TransientBaseComponent<ThumbnailComponentJsonModel>(client, model), IThumbnailComponent
{
    [field: MaybeNull]
    public IUnfurledMediaItem Media => field ??= new TransientUnfurledMediaItem(Model.Media);

    public string? Description => Model.Description.GetValueOrDefault();

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
