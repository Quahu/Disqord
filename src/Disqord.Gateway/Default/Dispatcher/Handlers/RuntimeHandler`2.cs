using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public sealed class RuntimeHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        private readonly Func<IGatewayApiClient, TModel, ValueTask<TEventArgs>> _func;

        public RuntimeHandler(Func<IGatewayApiClient, TModel, ValueTask<TEventArgs>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        public override ValueTask<TEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
            => _func(shard, model);
    }
}
