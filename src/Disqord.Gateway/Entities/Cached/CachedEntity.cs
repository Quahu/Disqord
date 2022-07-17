using System;
using System.ComponentModel;
using Qommon;

namespace Disqord.Gateway;

public abstract class CachedEntity : ICachedEntity, ICloneable
{
    /// <inheritdoc/>
    public IGatewayClient Client { get; }

    protected CachedEntity(IGatewayClient client)
    {
        Guard.IsNotNull(client);

        Client = client;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual object Clone()
        => MemberwiseClone();

    /// <inheritdoc/>
    public override string ToString()
        => this.GetString();
}