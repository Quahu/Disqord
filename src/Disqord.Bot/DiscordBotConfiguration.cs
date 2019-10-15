using System;
using System.Collections.Generic;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class DiscordBotConfiguration : DiscordClientConfiguration
    {
        public CommandService CommandService { get; set; }

        public Func<DiscordBot, IServiceProvider> ProviderFactory { get; set; }

        public IEnumerable<string> Prefixes { get; set; }

        public bool HasMentionPrefix { get; set; } = true;

        public static new DiscordBotConfiguration Default => new DiscordBotConfiguration();
    }
}
