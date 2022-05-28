using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    internal async ValueTask<bool> ProcessCommandsAsync(InteractionReceivedEventArgs e)
    {
        if (e.Interaction is not IApplicationCommandInteraction interaction)
            return false;

        // We create a text command context for Qmmands.
        IDiscordApplicationCommandContext context;
        try
        {
            context = CreateApplicationCommandContext(interaction);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while creating the application command context.");
            return false;
        }

        // We check the before execution callback, by default returns true.
        if (!await InvokeBeforeExecutedAsync(context).ConfigureAwait(false))
        {
            await DisposeContextAsync(context).ConfigureAwait(false);
            return false;
        }

        await ExecuteAsync(context);
        return true;
    }
}
