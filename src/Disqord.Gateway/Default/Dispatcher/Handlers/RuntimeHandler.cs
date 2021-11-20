using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler : Handler<JsonModel, EventArgs>
    {
        private readonly Func<IGatewayApiClient, JsonModel, ValueTask> _func;

        public RuntimeHandler(Func<IGatewayApiClient, JsonModel, ValueTask> func)
        {
            Guard.IsNotNull(func);

            _func = func;
        }

        public override async ValueTask<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, JsonModel model)
        {
            await _func(shard, model).ConfigureAwait(false);
            return null;
        }
    }
}
