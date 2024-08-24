using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public static class DiscordClientServiceCollectionExtensions
{
    public static IServiceCollection AddShardCoordinator<TShardCoordinator>(this IServiceCollection services)
        where TShardCoordinator : class, IShardCoordinator
    {
        services.TryAddSingleton<IShardCoordinator, TShardCoordinator>();
        return services;
    }
}
