using System.ComponentModel;
using Qommon;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class GatewayEntityExtensions
{
    internal static IGatewayClient GetGatewayClient(this IClientEntity entity)
    {
        Guard.IsNotNull(entity);

        return Guard.IsAssignableToType<IGatewayClient>(entity.Client);
    }
}
