using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class IntegrationCreateDispatchHandler : DispatchHandler<IntegrationCreateJsonModel, IntegrationCreatedEventArgs>
{
    public override ValueTask<IntegrationCreatedEventArgs?> HandleDispatchAsync(IShard shard, IntegrationCreateJsonModel model)
    {
        var integration = new TransientIntegration(Client, model.GuildId, model);
        var e = new IntegrationCreatedEventArgs(integration);
        return new(e);
    }
}
