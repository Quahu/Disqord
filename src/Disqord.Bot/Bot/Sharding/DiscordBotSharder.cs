using System;
using Disqord.Sharding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qmmands;

namespace Disqord.Bot.Sharding
{
    public class DiscordBotSharder : DiscordBotBase
    {
        public DiscordBotSharder(
            IOptions<DiscordBotSharderConfiguration> options,
            ILogger<DiscordBotSharder> logger,
            IPrefixProvider prefixes,
            ICommandQueue queue,
            CommandService commands,
            IServiceProvider services,
            DiscordClientSharder client)
            : base(logger, prefixes, queue, commands, services, client)
        {

        }
    }
}
