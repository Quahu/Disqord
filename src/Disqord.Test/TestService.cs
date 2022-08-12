using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Text;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;

namespace Disqord.Test
{
    public class TestService : DiscordBotService
    {
        protected override ValueTask OnReady(ReadyEventArgs e)
        {
            Logger.LogInformation("Ready fired for {ShardId}!", e.ShardId);
            return default;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitUntilReadyAsync(stoppingToken);
            Logger.LogInformation("Client says it's ready which is really cool.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // long-running background logic here
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
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

        protected override ValueTask OnCommandNotFound(IDiscordCommandContext context)
        {
            // Fired when an attempt is made to execute a command but it's not a valid one.
            if (context is IDiscordTextCommandContext textContext)
                Logger.LogInformation("Text command not found: {0}", textContext.Message.Content);

            return default;
        }
    }
}
