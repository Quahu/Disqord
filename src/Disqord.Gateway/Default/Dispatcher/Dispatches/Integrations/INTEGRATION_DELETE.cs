using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class IntegrationDeleteHandler : Handler<IntegrationDeleteJsonModel, IntegrationDeletedEventArgs>
    {
        public override ValueTask<IntegrationDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, IntegrationDeleteJsonModel model)
        {
            var e = new IntegrationDeletedEventArgs(model.GuildId, model.Id, model.ApplicationId.GetValueOrNullable());
            return new(e);
        }
    }
}
