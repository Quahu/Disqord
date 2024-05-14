using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class EntitlementCreateDispatchHandler : DispatchHandler<EntitlementJsonModel, EntitlementCreatedEventArgs>
{
    public override ValueTask<EntitlementCreatedEventArgs?> HandleDispatchAsync(IShard shard, EntitlementJsonModel model)
    {
        var entitlement = new TransientEntitlement(Client, model);
        var e = new EntitlementCreatedEventArgs(entitlement);
        return new(e);
    }
}
