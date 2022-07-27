using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Qommon.Binding;
using Qommon.Collections.Synchronized;
using Qommon.Events;

namespace Disqord.Gateway.Default.Dispatcher;

public abstract class DispatchHandler : IBindable<DefaultGatewayDispatcher>
{
    public DefaultGatewayDispatcher Dispatcher => _binder.Value;

    protected ILogger Logger => Dispatcher.Logger;

    protected IGatewayClient Client => Dispatcher.Client;

    protected IGatewayCacheProvider CacheProvider => Client.CacheProvider;

    private readonly Binder<DefaultGatewayDispatcher> _binder;

    private protected DispatchHandler()
    {
        _binder = new Binder<DefaultGatewayDispatcher>(this);
    }

    public virtual void Bind(DefaultGatewayDispatcher value)
    {
        _binder.Bind(value);
    }

    public abstract ValueTask HandleDispatchAsync(IShard shard, IJsonNode data);

    private protected static readonly ISynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, IAsynchronousEvent>> EventsByDispatcher = new SynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, IAsynchronousEvent>>(1);
    private protected static readonly PropertyInfo[] EventProperties;

    static DispatchHandler()
    {
        EventProperties = typeof(DefaultGatewayDispatcher).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(AsynchronousEvent<>))
            .ToArray();
    }
}
