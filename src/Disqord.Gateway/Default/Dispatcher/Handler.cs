using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Events;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public abstract class Handler : IBindable<DefaultGatewayDispatcher>
    {
        public DefaultGatewayDispatcher Dispatcher => _binder.Value;

        protected IGatewayClient Client => Dispatcher.Client;

        protected IGatewayCacheProvider CacheProvider => Client.CacheProvider;

        protected ILogger Logger => Dispatcher.Logger;

        private readonly Binder<DefaultGatewayDispatcher> _binder;

        private protected Handler()
        {
            _binder = new Binder<DefaultGatewayDispatcher>(this);
        }

        public virtual void Bind(DefaultGatewayDispatcher value)
        {
            _binder.Bind(value);
        }

        public abstract Task HandleDispatchAsync(IGatewayApiClient shard, IJsonToken data);

        private protected static readonly ISynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, AsynchronousEvent>> _eventsByDispatcher = new SynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, AsynchronousEvent>>(1);
        private protected static readonly PropertyInfo[] _eventsProperties;

        static Handler()
        {
            _eventsProperties = typeof(DefaultGatewayDispatcher).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(AsynchronousEvent<>))
                .ToArray();
        }
    }
}
