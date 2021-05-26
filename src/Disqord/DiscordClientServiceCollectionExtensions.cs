using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Extensions.Interactivity;
using Disqord.Gateway;
using Disqord.Rest;
using Disqord.Webhook;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord
{
    public static class DiscordClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordClient(this IServiceCollection services, Action<DiscordClientConfiguration> configure = null)
        {
            if (services.TryAddSingleton<DiscordClient>())
            {
                services.TryAddSingleton<DiscordClientBase>(x => x.GetRequiredService<DiscordClient>());
                services.AddOptions<DiscordClientConfiguration>();

                if (configure != null)
                    services.Configure(configure);
            }

            services.AddInteractivity();
            services.AddGatewayClient();
            services.AddRestClient();
            services.AddWebhookClientFactory();

            return services;
        }
    }
}
