using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public sealed class DelegateDispatchHandler<TModel> : DispatchHandler<TModel, EventArgs>
    where TModel : JsonModel
{
    private readonly Func<IShard, TModel, ValueTask> _func;

    public DelegateDispatchHandler(Func<IShard, TModel, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override async ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, TModel model)
    {
        await _func(shard, model).ConfigureAwait(false);
        return null;
    }
}
