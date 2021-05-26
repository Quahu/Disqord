using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class RuntimeRawHandler : Handler
    {
        private readonly Func<IGatewayApiClient, IJsonNode, ValueTask> _func;

        public RuntimeRawHandler(Func<IGatewayApiClient, IJsonNode, ValueTask> func)
        {
            _func = func;
        }

        public override ValueTask HandleDispatchAsync(IGatewayApiClient shard, IJsonNode data)
            => _func(shard, data);
    }
}
