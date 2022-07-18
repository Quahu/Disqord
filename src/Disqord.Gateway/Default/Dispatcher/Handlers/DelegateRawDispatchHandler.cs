using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class DelegateRawDispatchHandler : DispatchHandler
{
    private readonly Func<IGatewayApiClient, IJsonNode, ValueTask> _func;

    public DelegateRawDispatchHandler(Func<IGatewayApiClient, IJsonNode, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override ValueTask HandleDispatchAsync(IGatewayApiClient shard, IJsonNode data)
    {
        return _func(shard, data);
    }
}
