using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Gateway.Api.Default;

public class DefaultShardFactory : IShardFactory
{
    public IGatewayApiClient ApiClient => _binder.Value;

    private readonly IServiceProvider _services;
    private readonly Binder<IGatewayApiClient> _binder;

    public DefaultShardFactory(
        IOptions<DefaultShardFactoryConfiguration> options,
        IServiceProvider services)
    {
        _services = services;

        _binder = new(this);
    }

    public void Bind(IGatewayApiClient value)
    {
        _binder.Bind(value);
    }

    protected virtual IGatewayRateLimiter CreateRateLimiter()
    {
        return (RateLimiterFactory(_services, null) as IGatewayRateLimiter)!;
    }

    protected virtual IGatewayHeartbeater CreateHeartbeater()
    {
        return (HeartbeaterFactory(_services, null) as IGatewayHeartbeater)!;
    }

    protected virtual IGateway CreateGateway()
    {
        return (GatewayFactory(_services, null) as IGateway)!;
    }

    protected virtual IShard CreateShard(ShardId id, IGatewayRateLimiter rateLimiter, IGatewayHeartbeater heartbeater, IGateway gateway)
    {
        return (ShardFactory(_services, new object[] { id, rateLimiter, heartbeater, gateway }) as IShard)!;
    }

    public virtual IShard Create(ShardId id)
    {
        var rateLimiter = CreateRateLimiter();
        var heartbeater = CreateHeartbeater();
        var gateway = CreateGateway();
        var shard = CreateShard(id, rateLimiter, heartbeater, gateway);
        return shard;
    }

    protected static readonly ObjectFactory RateLimiterFactory;
    protected static readonly ObjectFactory HeartbeaterFactory;
    protected static readonly ObjectFactory GatewayFactory;
    protected static readonly ObjectFactory ShardFactory;

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
