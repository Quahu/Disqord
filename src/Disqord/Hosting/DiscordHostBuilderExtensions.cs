using System;
using System.Linq;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Hosting
{
    public static class DiscordHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordClient(this IHostBuilder builder, Action<HostBuilderContext, DiscordHostingContext> configure = null)
        {
            builder.ConfigureServices((context, x) =>
            {
                var discordContext = new DiscordHostingContext();
                configure?.Invoke(context, discordContext);

                if (!x.Any(x => x.ServiceType == typeof(Token)))
                {
                    var token = new BotToken(discordContext.Token);
                    x.AddToken(token);
                }

                if (discordContext.Intents != default)
                    x.Configure<DefaultGatewayApiClientConfiguration>(x => x.Intents = discordContext.Intents);

                x.AddDiscordClient();
                x.AddHostedService<DiscordHostedService>();
            });

            return builder;
        }
    }
}
