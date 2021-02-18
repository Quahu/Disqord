using System;
using Disqord.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qmmands;

namespace Disqord.Bot
{
    public static class DiscordBotServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordBot(this IServiceCollection services, Action<DiscordBotConfiguration> action = null)
        {
            services.AddDiscordClient();

            if (services.TryAddSingleton<DiscordBot>())
            {
                services.TryAddSingleton<DiscordBotBase>(x => x.GetRequiredService<DiscordBot>());
                services.Replace<DiscordClientBase>(x => x.GetRequiredService<DiscordBotBase>());
                services.AddOptions<DiscordBotConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            services.AddCommands();

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services, Action<CommandServiceConfiguration> action = null)
        {
            if (services.TryAddSingleton(x =>
            {
                var options = x.GetRequiredService<IOptions<CommandServiceConfiguration>>();
                return new CommandService(options.Value);
            }))
            {
                services.AddOptions<CommandServiceConfiguration>();

                if (action != null)
                    services.Configure(action);
            }

            return services;
        }
    }
}
