using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Bot.Commands.Text;
using Disqord.DependencyInjection.Extensions;
using Disqord.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Disqord.Bot.Hosting;

public static class DiscordBotHostBuilderExtensions
{
    public static IHostBuilder ConfigureDiscordBot(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext> configure)
    {
        return builder.ConfigureDiscordBot<DiscordBot>(configure);
    }

    public static IHostBuilder ConfigureDiscordBot<TDiscordBot>(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext> configure)
        where TDiscordBot : DiscordBot
    {
        return builder.ConfigureDiscordBot<TDiscordBot, DiscordBotConfiguration>(configure);
    }

    public static IHostBuilder ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext> configure)
        where TDiscordBot : DiscordBot
        where TDiscordBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        builder.ConfigureServices((context, services) =>
        {
            var discordContext = new DiscordBotHostingContext();
            configure?.Invoke(context, discordContext);

            ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(services, discordContext);
        });

        return builder;
    }

    public static void ConfigureDiscordBot(this IHostApplicationBuilder builder, DiscordBotHostingContext configuration)
    {
        builder.ConfigureDiscordBot<DiscordBot>(configuration);
    }

    public static void ConfigureDiscordBot<TDiscordBot>(this IHostApplicationBuilder builder, DiscordBotHostingContext configuration)
        where TDiscordBot : DiscordBot
    {
        builder.ConfigureDiscordBot<TDiscordBot, DiscordBotConfiguration>(configuration);
    }

    public static void ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(this IHostApplicationBuilder builder, DiscordBotHostingContext configuration)
        where TDiscordBot : DiscordBot
        where TDiscordBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(builder.Services, configuration);
    }

    private static void ConfigureDiscordBot<TDiscordBot, TDiscordBotConfiguration>(IServiceCollection services, DiscordBotHostingContext configuration)
        where TDiscordBot : DiscordBot
        where TDiscordBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        services.AddDiscordBot<TDiscordBot>();
        services.ConfigureDiscordBotServices<TDiscordBotConfiguration>(configuration);
    }

    internal static void ConfigureDiscordBotServices<TBotConfiguration>(this IServiceCollection services, DiscordBotHostingContext configuration)
        where TBotConfiguration : DiscordBotBaseConfiguration, new()
    {
        if (configuration.OwnerIds != null || configuration.ApplicationId != null)
        {
            services.Configure<TBotConfiguration>(x =>
            {
                x.OwnerIds = configuration.OwnerIds;
                x.ApplicationId = configuration.ApplicationId;
            });
        }

        services.AddSingleton<IConfigureOptions<DefaultPrefixProviderConfiguration>>(services => new ConfigureOptions<DefaultPrefixProviderConfiguration>(x =>
        {
            var prefixes = new List<IPrefix>();
            if (configuration.UseMentionPrefix)
            {
                if (services.GetService<Token>() is not BotToken botToken)
                    throw new InvalidOperationException("The mention prefix cannot be used without a bot token set.");

                prefixes.Add(new MentionPrefix(botToken.Id));
            }

            if (configuration.Prefixes != null)
                prefixes.AddRange(configuration.Prefixes.Select(x => new StringPrefix(x)));

            x.Prefixes = prefixes;
        }));

        services.AddHostedService<DiscordBotSetupService>();

        services.TryAddSingleton<DiscordBotMasterService>();

        services.ConfigureDiscordClientServices(configuration);
    }
}
