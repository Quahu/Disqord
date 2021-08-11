using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;

namespace Disqord.Test
{
    public class TestService : DiscordBotService
    {
        protected override ValueTask OnReady(ReadyEventArgs e)
        {
            Logger.LogInformation("Ready fired!");
            return default;
        }

        protected override async ValueTask OnInteractionReceived(InteractionReceivedEventArgs e)
        {
            if (e.Interaction is ITextCommandInteraction textCommandInteraction)
            {
                switch (textCommandInteraction.CommandName)
                {
                    case "echo":
                    {
                        var text = textCommandInteraction.Options.GetValueOrDefault("text")?.Value as string;
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionResponse().WithContent(text).WithAllowedMentions(LocalAllowedMentions.None));
                        break;
                    }
                }
            }
            else if (e.Interaction is IContextMenuInteraction contextMenuInteraction)
            {
                switch (contextMenuInteraction.CommandName)
                {
                    case "Rate Message" when contextMenuInteraction.CommandType == ApplicationCommandType.Message:
                    {
                        var message = contextMenuInteraction.Entities.Messages.GetValueOrDefault(contextMenuInteraction.TargetId);
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionResponse()
                            .WithContent("I rate it 2/10.")
                            .AddEmbed(new LocalEmbed()
                                .WithDescription(message.Content))
                            .WithAllowedMentions(LocalAllowedMentions.None));
                        break;
                    }
                    case "Give Cookie" when contextMenuInteraction.CommandType == ApplicationCommandType.User:
                    {
                        var user = contextMenuInteraction.Entities.Users.GetValueOrDefault(contextMenuInteraction.TargetId);
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionResponse().WithContent($"{user.Mention} 🍪."));
                        break;
                    }
                }
            }
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

        protected override ValueTask OnCommandNotFound(DiscordCommandContext context)
        {
            // Fired when an attempt is made to execute a command but it's not a valid one.
            Logger.LogInformation("Command not found: {0}", context.Message.Content);
            return default;
        }
    }
}
