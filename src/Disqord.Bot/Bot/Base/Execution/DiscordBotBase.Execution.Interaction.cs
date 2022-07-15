using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    internal async ValueTask<bool> ProcessCommandsAsync(InteractionReceivedEventArgs e)
    {
        if (e.Interaction is not IApplicationCommandInteraction and not IComponentInteraction and not IModalSubmitInteraction)
            return false;

        // We create a command context for Qmmands.
        IDiscordCommandContext context;
        try
        {
            context = CreateInteractionCommandContext(e.Interaction);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while creating the interaction command context.");
            return false;
        }

        // We run the common execution logic.
        await ExecuteAsync(context).ConfigureAwait(false);
        return true;
    }
}
