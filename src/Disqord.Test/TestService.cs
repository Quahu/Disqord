using System;
using System.Collections.Generic;
using System.Linq;
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
            if (e.Interaction is ISlashCommandInteraction textCommandInteraction)
            {
                switch (textCommandInteraction.CommandName)
                {
                    case "echo":
                    {
                        var textOption = textCommandInteraction.Options.GetValueOrDefault("text");
                        var text = textOption?.Value as string;
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionMessageResponse().WithIsEphemeral().WithContent(text).WithAllowedMentions(LocalAllowedMentions.None).WithIsTextToSpeech());
                        break;
                    }
                    case "coinflip":
                    {
                        var random = new Random();
                        var rawAmount = textCommandInteraction.Options.GetValueOrDefault("amount")?.Value;
                        var amount = 1;
                        if (rawAmount != null)
                            amount = (int) Math.Clamp(Convert.ToInt64(rawAmount), 1, 100);

                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionMessageResponse()
                            .WithContent(string.Join(", ", Enumerable.Range(0, amount).Select(x => random.Next(2) == 1 ? "heads" : "tails"))));

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
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionMessageResponse()
                            .WithContent("I rate it 2/10.")
                            .AddEmbed(new LocalEmbed()
                                .WithDescription(message.Content))
                            .WithAllowedMentions(LocalAllowedMentions.None));

                        break;
                    }
                    case "Give Cookie" when contextMenuInteraction.CommandType == ApplicationCommandType.User:
                    {
                        var user = contextMenuInteraction.Entities.Users.GetValueOrDefault(contextMenuInteraction.TargetId);
                        await e.Interaction.Response().SendMessageAsync(new LocalInteractionMessageResponse().WithContent($"{user.Mention} 🍪."));
                        break;
                    }
                }
            }
            else if (e.Interaction is IAutoCompleteInteraction autoCompleteInteraction)
            {
                switch (autoCompleteInteraction.CommandName)
                {
                    case "echo":
                    {
                        var textOption = autoCompleteInteraction.Options["text"];
                        if (!textOption.IsFocused)
                            return;

                        var choices = new List<LocalSlashCommandOptionChoice>
                        {
                            new LocalSlashCommandOptionChoice().WithName("hi").WithValue("hi"),
                            new LocalSlashCommandOptionChoice().WithName("hello").WithValue("hello")
                        };

                        var text = textOption.Value as string;
                        if (!string.IsNullOrWhiteSpace(text))
                            choices.Add(new LocalSlashCommandOptionChoice().WithName(text).WithValue(text));

                        await e.Interaction.Response().AutoCompleteAsync(choices);
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
