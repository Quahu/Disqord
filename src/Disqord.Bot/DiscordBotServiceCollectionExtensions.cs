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
            services.AddDiscordBot<DiscordBot>();

            var options = services.AddOptions<DiscordBotConfiguration>();
            if (configure != null)
                options.Configure(configure);

            return services;
        }

        public static IServiceCollection AddDiscordBot<TDiscordBot>(this IServiceCollection services)
            where TDiscordBot : DiscordBot
        {
            services.AddDiscordClient();

            if (services.TryAddSingleton<TDiscordBot>())
            {
                if (typeof(TDiscordBot) != typeof(DiscordBot))
                    services.TryAddSingleton<DiscordBot>(x => x.GetRequiredService<TDiscordBot>());

                services.TryAddSingleton<DiscordBotBase>(x => x.GetRequiredService<TDiscordBot>());
                services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(x => x.GetRequiredService<DiscordBotBase>()));
            }

            // TODO: add bot services efficiently

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

        public static IServiceCollection AddCommandQueue(this IServiceCollection services, Action<DefaultCommandQueueConfiguration> configure = null)
        {
            if (services.TryAddSingleton<ICommandQueue, DefaultCommandQueue>())
            {
                var options = services.AddOptions<DefaultCommandQueueConfiguration>();
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

        public static IServiceCollection AddCommandContextAccessor(this IServiceCollection services)
        {
            services.AddScoped<ICommandContextAccessor, DefaultCommandContextAccessor>();
            return services;
        }
    }
}
