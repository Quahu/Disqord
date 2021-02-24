using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord
{
    public static class DiscordClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordClient(this IServiceCollection services, Action<DiscordClientConfiguration>? configure = null)
        {
            if (services.TryAddSingleton<DiscordClient>())
            {
                services.TryAddSingleton<DiscordClientBase>(x => x.GetRequiredService<DiscordClient>());
                services.AddOptions<DiscordClientConfiguration>();

                if (configure != null)
                    services.Configure(configure);
            }

            services.AddDiscordApiClient();
            services.AddGatewayClient();
            services.AddRestClient();

            return services;
        }

        public static IServiceCollection AddDiscordApiClient(this IServiceCollection services, Action<DiscordApiClientConfiguration>? configure = null)
        {
            if (services.TryAddSingleton<DiscordApiClient>())
            {
                services.AddOptions<DiscordApiClientConfiguration>();

                if (configure != null)
                    services.Configure(configure);
            }

            services.AddGatewayApiClient();
            services.AddRestApiClient();

            return services;
        }
    }
}
