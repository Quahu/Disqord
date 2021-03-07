using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    internal class InterceptingHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        private readonly Handler<TModel, TEventArgs> _handler;
        private readonly Action<IGatewayApiClient, TModel> _func;

        public InterceptingHandler(Handler<TModel, TEventArgs> handler, Action<IGatewayApiClient, TModel> func)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _handler = handler;
            _func = func;
        }

        public override async Task<TEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
        {
            _func(shard, model);
            return await _handler.HandleDispatchAsync(shard, model).ConfigureAwait(false);
        }
    }
}
