using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public sealed class DelegateDispatchHandler : DispatchHandler<JsonModel, EventArgs>
{
    private readonly Func<IShard, JsonModel, ValueTask> _func;

    public DelegateDispatchHandler(Func<IShard, JsonModel, ValueTask> func)
    {
        Guard.IsNotNull(func);

        _func = func;
    }

    public override async ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, JsonModel model)
    {
        await _func(shard, model).ConfigureAwait(false);
        return null;
    }
}
