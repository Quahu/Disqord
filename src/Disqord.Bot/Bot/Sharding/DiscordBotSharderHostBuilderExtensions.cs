using System;
using System.ComponentModel;
using Disqord.Bot.Sharding;
using Disqord.DependencyInjection.Extensions;
using Disqord.Sharding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Bot.Hosting
{
    public static class DiscordBotSharderHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordBotSharder(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotSharderHostingContext> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordBotSharderHostingContext();
                configure?.Invoke(context, discordContext);

                services.ConfigureDiscordBotSharder(context, discordContext);
            });

            return builder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ConfigureDiscordBotSharder(this IServiceCollection services, HostBuilderContext context, DiscordBotHostingContext discordContext)
        {
            services.ConfigureDiscordBot(context, discordContext);
            services.ConfigureDiscordClientSharder(context, discordContext);

            if (services.TryAddSingleton<DiscordBotSharder>())
            {
                services.Remove<DiscordBot>();
                services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(x => x.GetRequiredService<DiscordBotSharder>()));
                services.Replace(ServiceDescriptor.Singleton<DiscordBotBase>(x => x.GetRequiredService<DiscordBotSharder>()));
            }
        }
    }
}
