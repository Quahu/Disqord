using System;
using Disqord.Hosting;
using Microsoft.Extensions.Hosting;

namespace Disqord.Bot.Hosting
{
    public static class DiscordBotHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordBot(this IHostBuilder builder, Action<HostBuilderContext, DiscordHostingContext> configure = null)
        {
            builder.ConfigureDiscordClient(configure);
            builder.ConfigureServices((context, x) =>
            {
                x.AddDiscordBot();
            });

            return builder;
        }
    }
}
