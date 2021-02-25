using System;
using System.ComponentModel;
using System.Linq;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Hosting
{
    public static class DiscordHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordClient(this IHostBuilder builder, Action<HostBuilderContext, DiscordClientHostingContext> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordClientHostingContext();
                configure?.Invoke(context, discordContext);

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

            services.AddDiscordClient();
            services.AddHostedService<DiscordHostedService>();
        }
    }
}
