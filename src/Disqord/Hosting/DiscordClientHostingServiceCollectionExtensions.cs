using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Hosting;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public static class DiscordClientHostingServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordClientService<TService>(this IServiceCollection services)
    {
        return services.AddDiscordClientService(typeof(TService));
    }

    public static IServiceCollection AddDiscordClientService(this IServiceCollection services, Type type)
    {
        services.AddSingleton(type);
        services.AddSingleton(typeof(DiscordClientService), x => x.GetService(type)!);
        services.AddSingleton(typeof(IHostedService), x => x.GetService(type)!);
        return services;
    }
}
