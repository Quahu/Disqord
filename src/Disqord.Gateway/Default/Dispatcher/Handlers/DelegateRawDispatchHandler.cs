using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class DelegateRawDispatchHandler : DispatchHandler
{
    private readonly Func<IShard, IJsonNode, ValueTask> _func;

    public DelegateRawDispatchHandler(Func<IShard, IJsonNode, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override ValueTask HandleDispatchAsync(IShard shard, IJsonNode data)
    {
        return _func(shard, data);
    }
}
