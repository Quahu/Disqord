using System;
using System.ComponentModel;

namespace Disqord.Gateway
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class GatewayEntityExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IGatewayClient GetGatewayClient(this IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not IGatewayClient client)
                throw new InvalidOperationException("This entity's client is not a gateway client implementation.");

            return client;
        }
    }
}
