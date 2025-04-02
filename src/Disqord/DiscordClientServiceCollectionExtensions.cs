using System.ComponentModel;
using Disqord.Api;
using Disqord.DependencyInjection.Extensions;
using Disqord.Extensions.Interactivity;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Webhook;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public static class DiscordClientServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordClient(this IServiceCollection services)
    {
        if (services.TryAddSingleton<DiscordClient>())
        {
            services.TryAddSingleton<DiscordClientBase>(services => services.GetRequiredService<DiscordClient>());
            services.AddShardCoordinator<LocalDiscordShardCoordinator>();
        }

        services.AddInteractivityExtension();
        services.AddGatewayClient();
        services.AddRestClient();
        services.AddWebhookClientFactory();

        return services;
    }

    public static IServiceCollection AddShardCoordinator<TShardCoordinator>(this IServiceCollection services)
        where TShardCoordinator : class, IShardCoordinator
    {
        services.TryAddSingleton<IShardCoordinator, TShardCoordinator>();
        return services;
    }
}
