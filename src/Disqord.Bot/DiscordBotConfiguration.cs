using System;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class DiscordBotConfiguration : DiscordClientConfiguration
    {
        public CommandService CommandService { get; set; }

        public Func<DiscordBotBase, IServiceProvider> ProviderFactory { get; set; }

        public static new DiscordBotConfiguration Default => new DiscordBotConfiguration();
    }
}
