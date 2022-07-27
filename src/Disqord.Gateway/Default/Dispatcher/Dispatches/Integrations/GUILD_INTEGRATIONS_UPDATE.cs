using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildIntegrationsUpdateDispatchHandler : DispatchHandler<GuildIntegrationsUpdateJsonModel, IntegrationsUpdatedEventArgs>
{
    public override ValueTask<IntegrationsUpdatedEventArgs?> HandleDispatchAsync(IShard shard, GuildIntegrationsUpdateJsonModel model)
    {
        var e = new IntegrationsUpdatedEventArgs(model.GuildId);
        return new(e);
    }
}
