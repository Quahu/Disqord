using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Bot.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Test
{
    public class TestService : DiscordBotService
    {
        public TestService(
            ILogger<TestService> logger,
            DiscordBotBase bot)
            : base(logger, bot)
        { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitUntilReadyAsync(stoppingToken);
            Logger.LogInformation("Client says it's ready which is really cool.");

            while (true)
            {
                // long-running background logic here
                await Task.Delay(TimeSpan.FromMinutes(5));
                Logger.LogInformation("5 minutes passed!");
            }
        }

        protected override ValueTask OnMessageReceived(BotMessageReceivedEventArgs e)
        {
            // Makes all messages containing `sax` not process commands.
            e.ProcessCommands = !e.Message.Content.Contains("sax");
            return default;
        }

        protected override ValueTask OnNonCommandReceived(BotMessageReceivedEventArgs e)
        {
            // Fired if the user doesn't provide a prefix or it's a system message etc.
            Logger.LogInformation("Received a non command: {0}", e.Message.Content);
            return default;
        }

        protected override ValueTask OnCommandNotFound(DiscordCommandContext context)
        {
            // Fired when an attempt is made to execute a command but it's not a valid one.
            Logger.LogInformation("Command not found: {0}", context.Message.Content);
            return default;
        }
    }
}
