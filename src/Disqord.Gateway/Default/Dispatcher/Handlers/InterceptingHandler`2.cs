using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class InterceptingHandler<TModel, TEventArgs> : Handler<TModel, TEventArgs>
        where TModel : JsonModel
        where TEventArgs : EventArgs
    {
        public Handler<TModel, TEventArgs> Handler { get; }

        private readonly Action<IGatewayApiClient, TModel> _func;

        public InterceptingHandler(Handler<TModel, TEventArgs> handler, Action<IGatewayApiClient, TModel> func)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (func == null)
                throw new ArgumentNullException(nameof(func));

            Handler = handler;
            _func = func;
        }

        public override ValueTask<TEventArgs> HandleDispatchAsync(IGatewayApiClient shard, TModel model)
        {
            _func(shard, model);
            return Handler.HandleDispatchAsync(shard, model);
        }
    }
}
