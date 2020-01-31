using System;
using Qmmands;

namespace Disqord.Bot
{
    public interface IDiscordBotBaseConfiguration
    {
        public CommandServiceConfiguration CommandServiceConfiguration { get; set; }

        public Func<DiscordBotBase, IServiceProvider> ProviderFactory { get; set; }
    }
}
