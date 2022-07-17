using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientEmbedAuthor : TransientEntity<EmbedAuthorJsonModel>, IEmbedAuthor
{
    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public string? Url => Model.Url.GetValueOrDefault();

    /// <inheritdoc/>
    public string? IconUrl => Model.IconUrl.GetValueOrDefault();

    /// <inheritdoc/>
    public string? ProxyIconUrl => Model.ProxyIconUrl.GetValueOrDefault();

    public TransientEmbedAuthor(EmbedAuthorJsonModel model)
        : base(model)
    { }
}
