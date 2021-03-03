using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Default;
using Disqord.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Sharding
{
    public static class DiscordClientHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordClientSharder(this IHostBuilder builder, Action<HostBuilderContext, DiscordClientSharderHostingContext> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordClientSharderHostingContext();
                configure?.Invoke(context, discordContext);

                services.ConfigureDiscordClientSharder(context, discordContext);
            });

            return builder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ConfigureDiscordClientSharder(this IServiceCollection services, HostBuilderContext context, DiscordClientHostingContext discordContext)
        {
            services.ConfigureDiscordClient(context, discordContext);

            var replacements = new Dictionary<Type, Type>
            {
                [typeof(IGateway)] = typeof(DefaultGateway),
                [typeof(IGatewayHeartbeater)] = typeof(DefaultGatewayHeartbeater),
                [typeof(IGatewayRateLimiter)] = typeof(DefaultGatewayRateLimiter)
            };
            for (var i = 0; i < services.Count; i++)
            {
                var service = services[i];
                if (service.ServiceType == typeof(IGatewayApiClient))
                {
                    services.RemoveAt(i);
                    i--;
                    continue;
                }

                if (replacements.TryGetValue(service.ServiceType, out var defaultType))
                {
                    services[i] = ServiceDescriptor.Scoped(service.ServiceType, defaultType);
                }
            }

            if (services.TryAddSingleton<DiscordClientSharder>())
            {
                services.Remove<DiscordClient>();
                services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(x => x.GetRequiredService<DiscordClientSharder>()));

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
}
