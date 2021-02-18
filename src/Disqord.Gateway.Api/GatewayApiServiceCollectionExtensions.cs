using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Gateway.Api
{
    public static class GatewayApiServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayApiClient(this IServiceCollection services, Action<DefaultGatewayApiClientConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayApiClient, DefaultGatewayApiClient>())
            {
                var options = services.AddOptions<DefaultGatewayApiClientConfiguration>();
                if (action != null)
                    options.Configure(action);
            }

            services.AddGatewayRateLimiter();
            services.AddGatewayHeartbeater();
            services.AddGateway();

            return services;
        }

        public static IServiceCollection AddGatewayRateLimiter(this IServiceCollection services, Action<DefaultGatewayRateLimiterConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayRateLimiter, DefaultGatewayRateLimiter>())
            {
                var options = services.AddOptions<DefaultGatewayRateLimiterConfiguration>();
                if (action != null)
                    options.Configure(action);
            }

            return services;
        }

        public static IServiceCollection AddGatewayHeartbeater(this IServiceCollection services, Action<DefaultGatewayHeartbeaterConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayHeartbeater, DefaultGatewayHeartbeater>())
            {
                var options = services.AddOptions<DefaultGatewayHeartbeaterConfiguration>();
                if (action != null)
                    options.Configure(action);
            }

            return services;
        }

        public static IServiceCollection AddGateway(this IServiceCollection services, Action<DefaultGatewayConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGateway, DefaultGateway>())
            {
                var options = services.AddOptions<DefaultGatewayConfiguration>();
                if (action != null)
                    options.Configure(action);
            }

            services.AddJsonSerializer();

            return services;
        }
    }
}
