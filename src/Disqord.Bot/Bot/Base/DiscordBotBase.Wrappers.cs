using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Microsoft.Extensions.Logging;
using Qmmands;
using Qommon.Disposal;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    private async ValueTask<bool> InvokeAfterExecutedAsync(IDiscordCommandContext context, IResult result)
    {
        try
        {
            return await OnAfterExecuted(context, result).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while executing the {0} callback.", nameof(OnAfterExecuted));
        }

        return false;
    }

    private async ValueTask InvokeFailedResultAsync(IDiscordCommandContext context, IResult result)
    {
        try
        {
            await OnFailedResult(context, result).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while executing the {0} callback.", nameof(OnFailedResult));
        }
    }

    private async ValueTask InvokeCommandResultAsync(IDiscordCommandContext context, IDiscordCommandResult result)
    {
        try
        {
            await OnCommandResult(context, result).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while executing the {0} callback.", nameof(OnCommandResult));
        }
    }

    private async ValueTask DisposeContextAsync(IDiscordCommandContext context)
    {
        try
        {
            await RuntimeDisposal.DisposeAsync(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while disposing of the command context.");
        }
    }
}
