using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ResumedDispatchHandler : DispatchHandler<GatewayPayloadJsonModel, EventArgs>
{
    public override ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, GatewayPayloadJsonModel model)
    {
        return new(EventArgs.Empty);
    }
}
