using System;
using Disqord.Bot;
using Qmmands;

namespace Disqord
{
    public class DiscordBotConfiguration : DiscordClientConfiguration, IDiscordBotBaseConfiguration
    {
        public CommandServiceConfiguration CommandServiceConfiguration { get; set; }

        public Func<DiscordBotBase, IServiceProvider> ProviderFactory { get; set; }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordBotConfiguration"/>.
        /// </summary>
        public DiscordBotConfiguration()
        { }

        public static new DiscordBotConfiguration Default => new DiscordBotConfiguration();
    }
}
