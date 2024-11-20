using System;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Bot.Commands.Application.Default;
using Disqord.Bot.Commands.Default;
using Disqord.Bot.Commands.Text;
using Disqord.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Qmmands.Text;

namespace Disqord.Bot;

public static class DiscordBotServiceCollectionExtensions
{
    public static IServiceCollection AddPrefixProvider<TPrefixProvider>(this IServiceCollection services)
        where TPrefixProvider : class, IPrefixProvider
    {
        services.TryAddSingleton<IPrefixProvider, TPrefixProvider>();
        return services;
    }

    public static IServiceCollection AddPrefixProvider(this IServiceCollection services, Action<DefaultPrefixProviderConfiguration>? configure = null)
    {
        if (services.TryAddSingleton<IPrefixProvider, DefaultPrefixProvider>())
        {
            var options = services.AddOptions<DefaultPrefixProviderConfiguration>();
            if (configure != null)
                options.Configure(configure);
        }

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddTextCommandService();
        return services;
    }

    public static IServiceCollection AddCommandContextAccessor(this IServiceCollection services)
    {
        services.TryAddScoped<ICommandContextAccessor, DefaultCommandContextAccessor>();
        return services;
    }

    public static IServiceCollection AddApplicationCommandLocalizer(this IServiceCollection services)
    {
        services.TryAddSingleton<IApplicationCommandLocalizer, DefaultApplicationCommandLocalizer>();
        return services;
    }

    public static IServiceCollection AddApplicationCommandCacheProvider(this IServiceCollection services)
    {
        services.TryAddSingleton<IApplicationCommandCacheProvider, DefaultApplicationCommandCacheProvider>();
        return services;
    }
}
