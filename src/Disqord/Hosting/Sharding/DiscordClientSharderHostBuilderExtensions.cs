using System;
using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Disqord.Extensions.Interactivity;
using Disqord.Gateway;
using Disqord.Hosting;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Sharding;

public static class DiscordClientHostBuilderExtensions
{
    public static IHostBuilder ConfigureDiscordClientSharder(this IHostBuilder builder, Action<HostBuilderContext, DiscordClientSharderHostingContext>? configure = null)
    {
        builder.ConfigureServices((context, services) =>
        {
            var discordContext = new DiscordClientSharderHostingContext();
            configure?.Invoke(context, discordContext);

            services.ConfigureDiscordClientSharder(context, discordContext);
            services.AddHostedService<DiscordClientRunnerService>();
        });

        return builder;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void ConfigureDiscordClientSharder(this IServiceCollection services, HostBuilderContext context, DiscordClientHostingContext discordContext)
    {
        services.ConfigureDiscordClient(context, discordContext);

        if (services.TryAddSingleton<DiscordClientSharder>())
        {
            services.AddSingleton<DiscordClientBase>(x => x.GetRequiredService<DiscordClientSharder>());

            services.AddInteractivityExtension();
            services.AddGatewayClient(ServiceLifetime.Scoped);
            services.AddRestClient();

            if (discordContext is IDiscordClientSharderConfiguration sharderConfiguration)
            {
                services.Configure<DiscordClientSharderConfiguration>(x =>
                {
                    x.ShardCount = sharderConfiguration.ShardCount;
                    x.ShardIds = sharderConfiguration.ShardIds;
                });
            }
        }

        services.TryAddSingleton<IShardFactory, DefaultShardFactory>();
    }
}
