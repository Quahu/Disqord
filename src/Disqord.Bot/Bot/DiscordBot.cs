using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qmmands;

namespace Disqord.Bot
{
    public class DiscordBot : DiscordBotBase
    {
        public DiscordBot(
            IOptions<DiscordBotConfiguration> options,
            ILogger<DiscordBot> logger,
            IPrefixProvider prefixes,
            CommandService commands,
            IServiceProvider services,
            DiscordClient client)
            : base(logger, prefixes, commands, services, client)
        {

        }
    }
}
