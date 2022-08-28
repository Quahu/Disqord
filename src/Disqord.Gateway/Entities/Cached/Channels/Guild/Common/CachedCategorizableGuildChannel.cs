using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="ICategorizableGuildChannel"/>
public abstract class CachedCategorizableGuildChannel : CachedGuildChannel, ICategorizableGuildChannel
{
    /// <inheritdoc/>
    public virtual Snowflake? CategoryId { get; private set; }

    protected CachedCategorizableGuildChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (model.ParentId.HasValue && !Type.IsThread())
            CategoryId = model.ParentId.Value;
    }
}
