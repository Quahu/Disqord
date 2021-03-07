using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler<TModel> : Handler<TModel, EventArgs>
        where TModel : JsonModel
    {
        private readonly Func<IGatewayApiClient, TModel, Task> _func;

        public RuntimeHandler(Func<IGatewayApiClient, TModel, Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        public override async Task<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
        {
            await _func(shard, model).ConfigureAwait(false);
            return null;
        }
    }
}
