using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Gateway;

public static class GatewayServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayClient(this IServiceCollection services)
    {
        services.AddGatewayApiClient();

        services.TryAddSingleton<IGatewayClient, DefaultGatewayClient>();
        services.TryAddSingleton<IGatewayCacheProvider, DefaultGatewayCacheProvider>();
        services.TryAddSingleton<IGatewayChunker, DefaultGatewayChunker>();
        services.TryAddSingleton<IAutomaticGatewayMemberChunkingSelector, DefaultAutomaticGatewayMemberChunkingSelector>();
        services.TryAddSingleton<IGatewayDispatcher, DefaultGatewayDispatcher>();
        return services;
    }

    public static IServiceCollection AddAutomaticMemberChunkingSelector<TSelector>(this IServiceCollection services)
        where TSelector : class, IAutomaticGatewayMemberChunkingSelector
    {
        services.Replace(ServiceDescriptor.Singleton<IAutomaticGatewayMemberChunkingSelector, TSelector>());
        return services;
    }
}
