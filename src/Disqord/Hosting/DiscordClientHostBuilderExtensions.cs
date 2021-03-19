using System;
using System.ComponentModel;
using System.Linq;
using Disqord.Gateway.Api.Default;
using Disqord.Gateway.Default;
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
                services.ConfigureDiscordClient(context, discordContext);
            });

            return builder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ConfigureDiscordClient(this IServiceCollection services, HostBuilderContext context, DiscordClientHostingContext discordContext)
        {
            if (!services.Any(x => x.ServiceType == typeof(Token)))
            {
                var token = new BotToken(discordContext.Token);
                services.AddToken(token);
            }

            if (discordContext.Intents != null)
                services.Configure<DefaultGatewayApiClientConfiguration>(x => x.Intents = discordContext.Intents.Value);

            services.Configure<DefaultGatewayDispatcherConfiguration>(x => x.ReadyEventDelayMode = discordContext.ReadyEventDelayMode);

            services.AddHostedService<DiscordClientRunnerService>();

            var serviceAssemblies = discordContext.ServiceAssemblies;
            if (serviceAssemblies != null)
            {
                for (var i = 0; i < serviceAssemblies.Count; i++)
                {
                    var types = serviceAssemblies[i].GetExportedTypes();
                    foreach (var type in types)
                    {
                        if (!typeof(DiscordClientService).IsAssignableFrom(type))
                            continue;

                        for (var j = 0; j < services.Count; j++)
                        {
                            var service = services[j];
                            if (service.ServiceType == typeof(IHostedService) && GetImplementationType(service) == type)
                                return;
                        }

                        services.AddSingleton(type);
                        services.AddSingleton(typeof(IHostedService), x => x.GetService(type));
                    }
                }
            }
        }

        internal static Type GetImplementationType(this ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationType != null)
            {
                return descriptor.ImplementationType;
            }
            else if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance.GetType();
            }
            else if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory.GetType().GenericTypeArguments[1];
            }

            return null;
        }
    }
}
