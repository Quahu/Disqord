using System;
using System.ComponentModel;

namespace Disqord.Gateway
{
    public abstract class CachedEntity : ICachedEntity, ICloneable
    {
        /// <inheritdoc/>
        public IGatewayClient Client { get; }

        protected CachedEntity(IGatewayClient client)
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
