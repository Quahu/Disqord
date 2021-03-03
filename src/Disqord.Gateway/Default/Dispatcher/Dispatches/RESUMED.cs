using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ResumedHandler : Handler<GatewayPayloadJsonModel, EventArgs>
    {
        public override Task<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, GatewayPayloadJsonModel model)
        {
            return Task.FromResult(EventArgs.Empty);
        }
    }
}
