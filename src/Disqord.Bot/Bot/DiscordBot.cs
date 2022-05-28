using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Bot;

public class DiscordBot : DiscordBotBase
{
    public DiscordBot(
        IOptions<DiscordBotConfiguration> options,
        ILogger<DiscordBot> logger,
        IServiceProvider services,
        DiscordClient client)
        : base(options, logger, services, client)
    { }
}
