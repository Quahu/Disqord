using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            services.AddDiscordBot<TDiscordBot>();
            services.ConfigureDiscordBot<TDiscordBotConfiguration>(context, discordContext);
            services.ConfigureDiscordClient(context, discordContext);
        });

        return builder;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void ConfigureDiscordBot<TBotConfiguration>(this IServiceCollection services, HostBuilderContext context, DiscordBotHostingContext discordContext)
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
}
