using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class InterceptingDispatchHandler<TModel, TEventArgs> : DispatchHandler<TModel, TEventArgs>
    where TModel : JsonModel
    where TEventArgs : EventArgs
{
    public DispatchHandler<TModel, TEventArgs> UnderlyingDispatchHandler { get; }

    private readonly Action<IShard, TModel> _func;

    public InterceptingDispatchHandler(DispatchHandler<TModel, TEventArgs> underlyingDispatchHandler, Action<IShard, TModel> func)
    {
        Guard.IsNotNull(underlyingDispatchHandler);
        Guard.IsNotNull(func);

        UnderlyingDispatchHandler = underlyingDispatchHandler;
        _func = func;
    }

    public override ValueTask<TEventArgs?> HandleDispatchAsync(IShard shard, TModel model)
    {
        _func(shard, model);
        return UnderlyingDispatchHandler.HandleDispatchAsync(shard, model);
    }
}
