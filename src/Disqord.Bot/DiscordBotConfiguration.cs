using System;
using Disqord.Bot;
using Qmmands;

namespace Disqord
{
    public class DiscordBotConfiguration : DiscordClientConfiguration, IDiscordBotBaseConfiguration
    {
        public CommandServiceConfiguration CommandServiceConfiguration { get; set; }
        public Func<DiscordBotBase, IServiceProvider> ProviderFactory { get; set; }

        public static new DiscordBotConfiguration Default => new DiscordBotConfiguration();
    }
}
