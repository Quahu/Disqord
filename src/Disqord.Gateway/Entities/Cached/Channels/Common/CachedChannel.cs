using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IChannel"/>
public abstract class CachedChannel : CachedSnowflakeEntity, IChannel
{
    /// <inheritdoc cref="INamableEntity.Name"/>
    public virtual string Name { get; private set; } = null!;

    /// <inheritdoc/>
    public ChannelType Type { get; private set; }

    protected CachedChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model.Id)
    {
        Update(model);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void Update(ChannelJsonModel model)
    {
        // Text channels can be changed to news channels and vice-versa.
        Type = model.Type;

        if (model.Name.HasValue)
            Name = model.Name.Value;
    }

    public override string ToString()
    {
        return Name;
    }
}
