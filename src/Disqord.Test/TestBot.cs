using System;
using Disqord.Bot;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Test
{
    public class TestBot : DiscordBot
    {
        public TestBot(IOptions<DiscordBotConfiguration> options, ILogger<DiscordBot> logger, IServiceProvider services, DiscordClient client)
            : base(options, logger, services, client)
        { }
    }
}
