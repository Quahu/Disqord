using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class IntegrationUpdateHandler : Handler<IntegrationUpdateJsonModel, IntegrationUpdatedEventArgs>
    {
        public override ValueTask<IntegrationUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, IntegrationUpdateJsonModel model)
        {
            var integration = new TransientIntegration(Client, model.GuildId, model);
            var e = new IntegrationUpdatedEventArgs(integration);
            return new(e);
        }
    }
}
