using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Gateway.Api;

public static class GatewayApiServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayApiClient(this IServiceCollection services, ServiceLifetime lifetime, Action<DefaultGatewayApiClientConfiguration>? action = null)
    {
        if (lifetime == ServiceLifetime.Singleton)
            services.TryAddSingleton<IGatewayApiClient, DefaultGatewayApiClient>();

        var options = services.AddOptions<DefaultGatewayApiClientConfiguration>();
        if (action != null)
            options.Configure(action);

        services.AddGatewayRateLimiter(lifetime);
        services.AddGatewayHeartbeater(lifetime);
        services.AddGateway(lifetime);

        return services;
    }

    public static IServiceCollection AddGatewayRateLimiter(this IServiceCollection services, ServiceLifetime lifetime, Action<DefaultGatewayRateLimiterConfiguration>? action = null)
    {
        if (services.TryAdd<IGatewayRateLimiter, DefaultGatewayRateLimiter>(lifetime))
        {
            var options = services.AddOptions<DefaultGatewayRateLimiterConfiguration>();
            if (action != null)
                options.Configure(action);
        }

        return services;
    }

    public static IServiceCollection AddGatewayHeartbeater(this IServiceCollection services, ServiceLifetime lifetime, Action<DefaultGatewayHeartbeaterConfiguration>? action = null)
    {
        if (services.TryAdd<IGatewayHeartbeater, DefaultGatewayHeartbeater>(lifetime))
        {
            var options = services.AddOptions<DefaultGatewayHeartbeaterConfiguration>();
            if (action != null)
                options.Configure(action);
        }

        return services;
    }

    public static IServiceCollection AddGateway(this IServiceCollection services, ServiceLifetime lifetime, Action<DefaultGatewayConfiguration>? action = null)
    {
        if (services.TryAdd<IGateway, DefaultGateway>(lifetime))
        {
            var options = services.AddOptions<DefaultGatewayConfiguration>();
            if (action != null)
                options.Configure(action);
        }

        services.AddJsonSerializer();
        services.AddWebSocketClientFactory();

        return services;
    }
}