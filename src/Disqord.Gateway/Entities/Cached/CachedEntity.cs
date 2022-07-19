using System;
using System.ComponentModel;
using Qommon;

namespace Disqord.Gateway;

public abstract class CachedEntity : ICachedEntity, ICloneable
{
    /// <inheritdoc/>
    public IGatewayClient Client { get; }

    /// <summary>
    ///     Instantiates a new <see cref="CachedEntity"/>.
    /// </summary>
    /// <param name="client"> The client that created this entity. </param>
    protected CachedEntity(IGatewayClient client)
    {
        Guard.IsNotNull(client);

        Client = client;
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual object Clone()
    {
        return MemberwiseClone();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.GetString();
    }
}
