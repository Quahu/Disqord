using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class EntitlementUpdateDispatchHandler : DispatchHandler<EntitlementJsonModel, EntitlementUpdatedEventArgs>
{
    public override ValueTask<EntitlementUpdatedEventArgs?> HandleDispatchAsync(IShard shard, EntitlementJsonModel model)
    {
        var entitlement = new TransientEntitlement(Client, model);
        var e = new EntitlementUpdatedEventArgs(entitlement);
        return new(e);
    }
}
