using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ResumedHandler : Handler<GatewayPayloadJsonModel, EventArgs>
    {
        public override Task<EventArgs> HandleDispatchAsync(GatewayPayloadJsonModel model)
        {
            return Task.FromResult(EventArgs.Empty);
        }
    }
}
