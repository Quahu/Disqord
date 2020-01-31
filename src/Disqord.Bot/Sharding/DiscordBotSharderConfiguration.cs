using System;
using Disqord.Sharding;
using Qmmands;

namespace Disqord.Bot.Sharding
{
    public class DiscordBotSharderConfiguration : DiscordSharderConfiguration, IDiscordBotBaseConfiguration
    {
        public CommandServiceConfiguration CommandServiceConfiguration { get; set; }

        public Func<DiscordBotBase, IServiceProvider> ProviderFactory { get; set; }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordBotSharderConfiguration"/>.
        /// </summary>
        public DiscordBotSharderConfiguration()
        { }

        public static DiscordBotSharderConfiguration Default => new DiscordBotSharderConfiguration();
    }
}
