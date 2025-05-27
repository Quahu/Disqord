using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="ISku"/>
public class TransientSku : TransientClientEntity<SkuJsonModel>, ISku
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public SkuType Type => Model.Type;

    /// <inheritdoc/>
    public Snowflake ApplicationId => Model.ApplicationId;

    /// <inheritdoc/>
    public string Slug => Model.Slug;

    /// <inheritdoc/>
    public SkuFlags Flags => Model.Flags;

    public TransientSku(IClient client, SkuJsonModel model)
        : base(client, model)
    { }
}
