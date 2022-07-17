using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class DelegateRawHandler : Handler
{
    private readonly Func<IGatewayApiClient, IJsonNode, ValueTask> _func;

    public DelegateRawHandler(Func<IGatewayApiClient, IJsonNode, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override ValueTask HandleDispatchAsync(IGatewayApiClient shard, IJsonNode data)
    {
        return _func(shard, data);
    }
}
