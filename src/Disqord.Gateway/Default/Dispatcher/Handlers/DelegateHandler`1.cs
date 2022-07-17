using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public sealed class DelegateHandler<TModel> : Handler<TModel, EventArgs>
    where TModel : JsonModel
{
    private readonly Func<IGatewayApiClient, TModel, ValueTask> _func;

    public DelegateHandler(Func<IGatewayApiClient, TModel, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override async ValueTask<EventArgs?> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
    {
        await _func(shard, model).ConfigureAwait(false);
        return null;
    }
}
