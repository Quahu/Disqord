using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Gateway.Api;

public static class GatewayApiServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayApiClient(this IServiceCollection services)
    {
        services.TryAddSingleton<IGatewayApiClient, DefaultGatewayApiClient>();
        services.TryAddSingleton<IShardFactory, DefaultShardFactory>();
        services.AddJsonSerializer();
        services.AddWebSocketClientFactory();
        return services;
    }
}
