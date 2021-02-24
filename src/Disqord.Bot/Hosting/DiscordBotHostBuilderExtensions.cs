using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Disqord.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Disqord.Bot.Hosting
{
    public static class DiscordBotHostBuilderExtensions
    {
        public static IHostBuilder ConfigureDiscordBot(this IHostBuilder builder, Action<HostBuilderContext, DiscordBotHostingContext> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                var discordContext = new DiscordBotHostingContext();
                configure?.Invoke(context, discordContext);

                services.ConfigureDiscordBot(context, discordContext);
            });

            return builder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ConfigureDiscordBot(this IServiceCollection services, HostBuilderContext context, DiscordBotHostingContext discordContext)
        {
            services.ConfigureDiscordClient(context, discordContext);

            if (!services.Any(x => x.ServiceType == typeof(IPrefixProvider)))
            {
                if (!discordContext.UseMentionPrefix && discordContext.Prefixes == null)
                    throw new InvalidOperationException($"No prefixes were specified and no {nameof(IPrefixProvider)} exists in services. Did you pass null prefixes by mistake?");

                var prefixes = new List<IPrefix>();
                // We have to use some messy code to create the MentionPrefix with the bot's ID.
                if (discordContext.UseMentionPrefix)
                {
                    // TODO: rethink
                    var botToken = services.FirstOrDefault(x => x.ImplementationInstance is BotToken)?.ImplementationInstance as BotToken;
                    if (botToken != null)
                    {
                        prefixes.Add(new MentionPrefix(botToken.Id));
                    }
                }

                if (discordContext.Prefixes != null)
                {
                    prefixes.AddRange(discordContext.Prefixes.Select(x => new StringPrefix(x)));
                }

                services.AddPrefixProvider(x => x.Prefixes = prefixes);
            }

            services.AddDiscordBot();
        }
    }
}
