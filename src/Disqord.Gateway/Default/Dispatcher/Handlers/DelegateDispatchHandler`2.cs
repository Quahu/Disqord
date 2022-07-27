using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public sealed class DelegateDispatchHandler<TModel, TEventArgs> : DispatchHandler<TModel, TEventArgs>
    where TModel : JsonModel
    where TEventArgs : EventArgs
{
    private readonly Func<IShard, TModel, ValueTask<TEventArgs?>> _func;

    public DelegateDispatchHandler(Func<IShard, TModel, ValueTask<TEventArgs?>> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override ValueTask<TEventArgs?> HandleDispatchAsync(IShard shard, TModel model)
    {
        return _func(shard, model);
    }
}
