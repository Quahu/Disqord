using System;

namespace Disqord.Gateway
{
    public abstract class GatewayEntity : IGatewayEntity
    {
        public IGatewayClient Client { get; }

        public GatewayEntity(IGatewayClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
        }
    }
}
