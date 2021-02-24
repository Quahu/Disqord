using System;
using Disqord.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qmmands;

namespace Disqord.Bot
{
    public static class DiscordBotServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordBot(this IServiceCollection services, Action<DiscordBotConfiguration> configure = null)
        {
            services.AddDiscordClient();

            if (services.TryAddSingleton<DiscordBot>())
            {
                services.TryAddSingleton<DiscordBotBase>(x => x.GetRequiredService<DiscordBot>());
                services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(x => x.GetRequiredService<DiscordBotBase>()));
                services.AddOptions<DiscordBotConfiguration>();

                if (configure != null)
                    services.Configure(configure);
            }

            services.AddPrefixProvider();
            services.AddCommands();

            return services;
        }

        public static IServiceCollection AddPrefixProvider<TPrefixProvider>(this IServiceCollection services)
            where TPrefixProvider : class, IPrefixProvider
        {
            services.AddSingleton<IPrefixProvider, TPrefixProvider>();
            return services;
        }

        public static IServiceCollection AddPrefixProvider(this IServiceCollection services, Action<DefaultPrefixProviderConfiguration> configure = null)
        {
            if (services.TryAddSingleton<IPrefixProvider, DefaultPrefixProvider>())
            {
                var options = services.AddOptions<DefaultPrefixProviderConfiguration>();
                if (configure != null)
                    options.Configure(configure);
            }

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services, Action<CommandServiceConfiguration> configure = null)
        {
            if (services.TryAddSingleton(x =>
            {
                var options = x.GetRequiredService<IOptions<CommandServiceConfiguration>>();
                return new CommandService(options.Value);
            }))
            {
                services.AddOptions<CommandServiceConfiguration>();
                if (configure != null)
                    services.Configure(configure);
            }

            return services;
        }
    }
}
