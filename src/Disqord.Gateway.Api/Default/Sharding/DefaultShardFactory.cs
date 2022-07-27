using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Gateway.Api.Default;

public class DefaultShardFactory : IShardFactory
{
    public ILogger Logger { get; }

    public IGatewayApiClient ApiClient => _binder.Value;

    private readonly IServiceProvider _services;
    private readonly Binder<IGatewayApiClient> _binder;

    public DefaultShardFactory(
        IOptions<DefaultShardFactoryConfiguration> options,
        ILogger<DefaultShardFactory> logger,
        IServiceProvider services)
    {
        Logger = logger;
        _services = services;

        _binder = new(this);
    }

    public void Bind(IGatewayApiClient value)
    {
        _binder.Bind(value);
    }

    public IShard Create(ShardId id)
    {
        var rateLimiter = (RateLimiterFactory(_services, null) as IGatewayRateLimiter)!;
        var heartbeater = (HeartbeaterFactory(_services, null) as IGatewayHeartbeater)!;
        var gateway = (GatewayFactory(_services, null) as IGateway)!;
        var shard = (ShardFactory(_services, new object[] { id, rateLimiter, heartbeater, gateway }) as IShard)!;
        return shard;
    }

    private static readonly ObjectFactory RateLimiterFactory;
    private static readonly ObjectFactory HeartbeaterFactory;
    private static readonly ObjectFactory GatewayFactory;
    private static readonly ObjectFactory ShardFactory;

    static DefaultShardFactory()
    {
        RateLimiterFactory = ActivatorUtilities.CreateFactory(typeof(DefaultGatewayRateLimiter),
            Array.Empty<Type>());

        HeartbeaterFactory = ActivatorUtilities.CreateFactory(typeof(DefaultGatewayHeartbeater),
            Array.Empty<Type>());

        GatewayFactory = ActivatorUtilities.CreateFactory(typeof(DefaultGateway),
            Array.Empty<Type>());

        ShardFactory = ActivatorUtilities.CreateFactory(typeof(DefaultShard),
            new[] { typeof(ShardId), typeof(IGatewayRateLimiter), typeof(IGatewayHeartbeater), typeof(IGateway) });
    }
}
