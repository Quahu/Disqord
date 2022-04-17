using System;
using System.ComponentModel;
using System.Linq;
using Disqord.DependencyInjection.Extensions;
using Disqord.Gateway.Api.Default;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Default;
using Disqord.Gateway.Models;
using Disqord.Http.Default;
using Disqord.WebSocket.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Hosting
{
    public static class DiscordClientHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordClient(this IHostBuilder builder, Action<HostBuilderContext, DiscordClientHostingContext> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordClientHostingContext();
                configure?.Invoke(context, discordContext);

                services.AddDiscordClient();
                services.AddHostedService<DiscordClientSetupService>();
                services.ConfigureDiscordClient(context, discordContext);
            });

            return builder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ConfigureDiscordClient(this IServiceCollection services, HostBuilderContext context, DiscordClientHostingContext discordContext)
        {
            services.Replace(ServiceDescriptor.Singleton<Token>(Token.Bot(discordContext.Token)));

            services.Configure<DefaultGatewayApiClientConfiguration>(x => x.Intents = discordContext.Intents);

            services.Configure<DefaultGatewayDispatcherConfiguration>(x => x.ReadyEventDelayMode = discordContext.ReadyEventDelayMode);

            services.TryAddSingleton<DiscordClientMasterService>();
            services.AddHostedService(x => x.GetRequiredService<DiscordClientMasterService>());

            var serviceAssemblies = discordContext.ServiceAssemblies;
            if (serviceAssemblies != null)
            {
                for (var i = 0; i < serviceAssemblies.Count; i++)
                {
                    var types = serviceAssemblies[i].GetExportedTypes();
                    foreach (var type in types)
                    {
                        if (type.IsAbstract)
                            continue;

                        if (!typeof(DiscordClientService).IsAssignableFrom(type))
                            continue;

                        var hasService = false;
                        for (var j = 0; j < services.Count; j++)
                        {
                            var service = services[j];
                            if (service.ServiceType != type && (service.ServiceType != typeof(IHostedService) || service.GetImplementationType() != type))
                                continue;

                            hasService = true;
                            break;
                        }

                        if (hasService)
                            continue;

                        services.AddDiscordClientService(type);
                    }
                }
            }

            var status = discordContext.Status;
            var activities = discordContext.Activities;
            if (status != null || activities != null)
            {
                var presence = new UpdatePresenceJsonModel
                {
                    Status = status ?? UserStatus.Online,
                    Activities = activities?.Select(activity => activity.ToModel()).ToArray() ?? Array.Empty<ActivityJsonModel>()
                };

                services.Configure<DefaultGatewayApiClientConfiguration>(x => x.Presence = presence);
            }

            var restProxy = discordContext.RestProxy;
            if (restProxy != null)
            {
                services.Configure<DefaultHttpClientFactoryConfiguration>(x => x.ClientConfiguration = client =>
                {
                    client.Proxy = restProxy;
                    client.UseProxy = true;
                });
            }

            var gatewayProxy = discordContext.GatewayProxy;
            if (gatewayProxy != null)
            {
                services.Configure<DefaultWebSocketClientFactoryConfiguration>(x => x.ClientConfiguration = client =>
                {
                    client.Proxy = gatewayProxy;
                });
            }

            services.AddHostedService<DiscordClientRunnerService>();
        }
    }
}
