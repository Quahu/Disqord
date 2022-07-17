using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientEmbedVideo : TransientEntity<EmbedVideoJsonModel>, IEmbedVideo
{
    /// <inheritdoc/>
    public string? Url => Model.Url.GetValueOrDefault();

    /// <inheritdoc/>
    public int? Width => Model.Width.GetValueOrNullable();

    /// <inheritdoc/>
    public int? Height => Model.Height.GetValueOrNullable();

    public TransientEmbedVideo(EmbedVideoJsonModel model)
        : base(model)
    { }
}
