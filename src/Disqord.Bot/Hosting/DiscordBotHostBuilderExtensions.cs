using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Disqord.Bot.Commands.Text;
using Disqord.DependencyInjection.Extensions;
using Disqord.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Disqord.Bot.Hosting;

public static class DiscordBotHostBuilderExtensions
{
    public static IHostBuilder ConfigureDiscordBot(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext>? configure = null)
    {
        return builder.ConfigureDiscordBot<DiscordBot>(configure);
    }

    public static IHostBuilder ConfigureDiscordBot<TDiscordBot>(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext>? configure = null)
        where TDiscordBot : DiscordBot
    {
        return builder.ConfigureDiscordBot<TDiscordBot, DiscordBotConfiguration>(configure);
    }

    public static IHostBuilder ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext>? configure = null)
        where TDiscordBot : DiscordBot
        where TDiscordBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        builder.ConfigureServices((context, services) =>
        {
            var discordContext = new DiscordBotHostingContext();
            configure?.Invoke(context, discordContext);

            services.AddDiscordBot<TDiscordBot, TDiscordBotConfiguration>(discordContext);
        });

        return builder;
    }

    public static IServiceCollection AddDiscordBot(this IServiceCollection services, DiscordBotHostingContext context)
    {
        return services.AddDiscordBot<DiscordBot>(context);
    }

    public static IServiceCollection AddDiscordBot<TDiscordBot>(this IServiceCollection services, DiscordBotHostingContext context)
        where TDiscordBot : DiscordBot
    {
        return services.AddDiscordBot<TDiscordBot, DiscordBotConfiguration>(context);
    }

    public static IServiceCollection AddDiscordBot<TDiscordBot, TDiscordBotConfiguration>(this IServiceCollection services, DiscordBotHostingContext context)
        where TDiscordBot : DiscordBot
        where TDiscordBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        services.AddDiscordBot<TDiscordBot>();
        services.ConfigureDiscordBot<TDiscordBotConfiguration>(context);
        services.ConfigureDiscordClient(context);
        return services;
    }

    internal static void ConfigureDiscordBot<TBotConfiguration>(this IServiceCollection services, DiscordBotHostingContext discordContext)
        where TBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        if (discordContext.OwnerIds != null || discordContext.ApplicationId != null)
        {
            services.Configure<TBotConfiguration>(x =>
            {
                x.OwnerIds = discordContext.OwnerIds;
                x.ApplicationId = discordContext.ApplicationId;
            });
        }

        services.AddSingleton<IConfigureOptions<DefaultPrefixProviderConfiguration>>(services => new ConfigureOptions<DefaultPrefixProviderConfiguration>(x =>
        {
            var prefixes = new List<IPrefix>();
            if (discordContext.UseMentionPrefix)
            {
                if (services.GetService<Token>() is not BotToken botToken)
                    throw new InvalidOperationException("The mention prefix cannot be used without a bot token set.");

                prefixes.Add(new MentionPrefix(botToken.Id));
            }

            if (discordContext.Prefixes != null)
                prefixes.AddRange(discordContext.Prefixes.Select(x => new StringPrefix(x)));

            x.Prefixes = prefixes;
        }));

        services.AddHostedService<DiscordBotSetupService>();

        services.TryAddSingleton<DiscordBotMasterService>();
    }

    internal static IServiceCollection AddDiscordBot<TDiscordBot>(this IServiceCollection services)
        where TDiscordBot : DiscordBot
    {
        services.AddDiscordClient();

        if (services.TryAddSingleton<TDiscordBot>())
        {
            var callCount = 0;

            TDiscordBot GetBotService(IServiceProvider services)
            {
                if (Interlocked.Increment(ref callCount) > 1)
                    throw new InvalidOperationException($"Disqord detected a circular dependency for the bot client of type '{typeof(TDiscordBot)}'. "
                        + "This means that most likely your prefix provider or another service depends on the bot client and vice versa.");

                var service = services.GetRequiredService<TDiscordBot>();
                Interlocked.Decrement(ref callCount);
                return service;
            }

            if (typeof(TDiscordBot) != typeof(DiscordBot))
                services.TryAddSingleton<DiscordBot>(GetBotService);

            services.TryAddSingleton<DiscordBotBase>(GetBotService);
            services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(GetBotService));
        }

        services.AddDiscordBotDependencies();
        return services;
    }

    internal static void AddDiscordBotDependencies(this IServiceCollection services)
    {
        services.AddPrefixProvider();
        services.AddCommands();
        services.AddCommandContextAccessor();
        services.AddApplicationCommandLocalizer();
        services.AddApplicationCommandCacheProvider();
    }
}
