using System;
using System.ComponentModel;

namespace Disqord.Gateway
{
    public abstract class CachedEntity : IGatewayEntity, ICloneable
    {
        public IGatewayClient Client { get; }

        public CachedEntity(IGatewayClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual object Clone()
            => MemberwiseClone();
    }
}
