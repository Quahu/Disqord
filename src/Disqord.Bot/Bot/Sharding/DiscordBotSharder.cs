using System;
using Disqord.Sharding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Bot.Sharding
{
    public class DiscordBotSharder : DiscordBotBase
    {
        public DiscordBotSharder(
            IOptions<DiscordBotSharderConfiguration> options,
            ILogger<DiscordBotSharder> logger,
            IServiceProvider services,
            DiscordClientSharder client)
            : base(options, logger, services, client)
        { }
    }
}
