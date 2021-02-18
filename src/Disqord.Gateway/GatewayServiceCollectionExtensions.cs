using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Gateway
{
    public static class GatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayClient(this IServiceCollection services, Action<DefaultGatewayClientConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayClient, DefaultGatewayClient>())
            {
                services.AddOptions<DefaultGatewayClientConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            services.AddGatewayApiClient();
            services.AddGatewayCacheProvider();
            services.AddGatewayDispatcher();

            return services;
        }

        public static IServiceCollection AddGatewayCacheProvider(this IServiceCollection services, Action<DefaultGatewayCacheProviderConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayCacheProvider, DefaultGatewayCacheProvider>())
            {
                services.AddOptions<DefaultGatewayCacheProviderConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            return services;
        }

        public static IServiceCollection AddGatewayDispatcher(this IServiceCollection services, Action<DefaultGatewayDispatcherConfiguration> action = null)
        {
            if (services.TryAddSingleton<IGatewayDispatcher, DefaultGatewayDispatcher>())
            {
                services.AddOptions<DefaultGatewayDispatcherConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            return services;
        }
    }
}
