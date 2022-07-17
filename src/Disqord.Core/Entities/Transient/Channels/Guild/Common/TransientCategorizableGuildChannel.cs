using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="ICategorizableGuildChannel"/>
public abstract class TransientCategorizableGuildChannel : TransientGuildChannel, ICategorizableGuildChannel
{
    /// <inheritdoc/>
    public virtual Snowflake? CategoryId => Model.ParentId.Value;

    protected TransientCategorizableGuildChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}