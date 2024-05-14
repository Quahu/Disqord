using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class EntitlementDeleteDispatchHandler : DispatchHandler<EntitlementJsonModel, EntitlementDeletedEventArgs>
{
    public override ValueTask<EntitlementDeletedEventArgs?> HandleDispatchAsync(IShard shard, EntitlementJsonModel model)
    {
        var entitlement = new TransientEntitlement(Client, model);
        var e = new EntitlementDeletedEventArgs(entitlement);
        return new(e);
    }
}
