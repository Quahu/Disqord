using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class InterceptingHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
    where TModel : JsonModel
    where TEventArgs : EventArgs
{
    public Handler<TModel, TEventArgs> UnderlyingHandler { get; }

    private readonly Action<IGatewayApiClient, TModel> _func;

    public InterceptingHandler(Handler<TModel, TEventArgs> underlyingHandler, Action<IGatewayApiClient, TModel> func)
    {
        Guard.IsNotNull(underlyingHandler);
        Guard.IsNotNull(func);

        UnderlyingHandler = underlyingHandler;
        _func = func;
    }

    public override ValueTask<TEventArgs?> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
    {
        _func(shard, model);
        return UnderlyingHandler.HandleDispatchAsync(shard, model);
    }
}
