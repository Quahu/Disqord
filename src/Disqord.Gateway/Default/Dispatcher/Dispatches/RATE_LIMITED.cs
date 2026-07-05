using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class RateLimitedDispatchHandler : DispatchHandler<RateLimitedJsonModel, GatewayRateLimitedEventArgs>
{
    public override ValueTask<GatewayRateLimitedEventArgs?> HandleDispatchAsync(IShard shard, RateLimitedJsonModel model)
    {
        return new(new GatewayRateLimitedEventArgs(shard.Id, model.Opcode, TimeSpan.FromSeconds(model.RetryAfter), model.Meta));
    }
}
