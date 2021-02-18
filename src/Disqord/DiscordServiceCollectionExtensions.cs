using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord
{
    public static class DiscordServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordClient(this IServiceCollection services, Action<DiscordClientConfiguration> action = null)
        {
            if (services.TryAddSingleton<DiscordClient>())
            {
                services.TryAddSingleton<DiscordClientBase>(x => x.GetRequiredService<DiscordClient>());
                services.AddOptions<DiscordClientConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            services.AddDiscordApiClient();
            services.AddGatewayClient();
            services.AddRestClient();

            return services;
        }

        public static IServiceCollection AddDiscordApiClient(this IServiceCollection services, Action<DiscordApiClientConfiguration> action = null)
        {
            if (services.TryAddSingleton<DiscordApiClient>())
            {
                services.AddOptions<DiscordApiClientConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            services.AddGatewayApiClient();
            services.AddRestApiClient();

            return services;
        }
    }
}
