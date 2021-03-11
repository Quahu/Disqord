using System;
using Disqord.Bot.Sharding;
using Disqord.DependencyInjection.Extensions;
using Disqord.Sharding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Bot.Hosting
{
    public static class DiscordBotSharderHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordBotSharder(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext> configure = null)
            => builder.ConfigureDiscordBotSharder<DiscordBotSharder>(configure);

        public static IHostBuilder ConfigureDiscordBotSharder<TDiscordBotSharder>(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotSharderHostingContext> configure = null)
            where TDiscordBotSharder : DiscordBotSharder
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordBotSharderHostingContext();
                configure?.Invoke(context, discordContext);

                services.ConfigureDiscordBot(context, discordContext);
                services.ConfigureDiscordClientSharder(context, discordContext);

                if (services.TryAddSingleton<TDiscordBotSharder>())
                {
                    services.Replace(ServiceDescriptor.Singleton<DiscordClientBase>(x => x.GetRequiredService<TDiscordBotSharder>()));
                    services.Replace(ServiceDescriptor.Singleton<DiscordBotBase>(x => x.GetRequiredService<TDiscordBotSharder>()));

                    if (typeof(TDiscordBotSharder) != typeof(DiscordBotSharder))
                        services.Replace(ServiceDescriptor.Singleton<DiscordBotSharder>(x => x.GetRequiredService<TDiscordBotSharder>()));
                }
            });

            return builder;
        }
    }
}
