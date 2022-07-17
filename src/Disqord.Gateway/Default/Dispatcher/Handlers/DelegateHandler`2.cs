using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public sealed class DelegateHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
    where TModel : JsonModel
    where TEventArgs : EventArgs
{
    private readonly Func<IGatewayApiClient, TModel, ValueTask<TEventArgs?>> _func;

    public DelegateHandler(Func<IGatewayApiClient, TModel, ValueTask<TEventArgs?>> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override ValueTask<TEventArgs?> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
    {
        return _func(shard, model);
    }
}
