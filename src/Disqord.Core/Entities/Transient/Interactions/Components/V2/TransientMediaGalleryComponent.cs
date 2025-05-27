using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientMediaGalleryComponent(IClient client, MediaGalleryComponentJsonModel model)
    : TransientBaseComponent<MediaGalleryComponentJsonModel>(client, model), IMediaGalleryComponent
{
    [field: MaybeNull]
    public IReadOnlyList<IMediaGalleryItem> Items => field ??= Model.Items.ToReadOnlyList(
        static model => new TransientMediaGalleryItem(model));
}
