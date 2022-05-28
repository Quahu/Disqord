using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    public async Task ExecuteAsync(IDiscordCommandContext context)
    {
        try
        {
            IResult result;
            try
            {
                result = await Commands.ExecuteAsync(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while attempting to execute commands.");
                return;
            }

            if (!await InvokeAfterExecutedAsync(context, result).ConfigureAwait(false))
                return;

            if (result.IsSuccessful)
            {
                if (result is IDiscordCommandResult commandResult)
                {
                    await InvokeCommandResultAsync(context, commandResult).ConfigureAwait(false);
                }

                return;
            }

            await InvokeFailedResultAsync(context, result).ConfigureAwait(false);

            if (result is ExceptionResult exceptionResult)
            {
                LogExceptionResult(context, exceptionResult);
            }
            else if (_masterService != null && result is CommandNotFoundResult)
            {
                await _masterService.HandleCommandNotFound(context).ConfigureAwait(false);
            }
        }
        finally
        {
            await DisposeContextAsync(context).ConfigureAwait(false);
        }
    }

    private void LogExceptionResult(IDiscordCommandContext context, ExceptionResult exceptionResult)
    {
        if (context.Command != null && exceptionResult.Exception is CommandContextTypeMismatchException mismatchException)
        {
            var message = "A command context type mismatch occurred while attempting to execute {0}. " +
                "The module expected {1}, but got {2}.";

            var expectedType = mismatchException.ExpectedType;
            var actualType = mismatchException.ActualType;
            var args = new List<object>(4)
            {
                context.Command.Name,
                expectedType,
                actualType
            };

            // If the expected type is a DiscordGuildCommandContext, the actual type is a DiscordCommandContext, and the module doesn't have guild restrictions.
            if (typeof(IDiscordGuildCommandContext).IsAssignableFrom(expectedType)
                && typeof(IDiscordCommandContext).IsAssignableFrom(actualType)
                && !CommandUtilities.EnumerateAllChecks(context.Command.Module).Any(x => x is RequireGuildAttribute))
            {
                message += " Did you forget to decorate the module with {3}?";
                args.Add(nameof(RequireGuildAttribute));
            }

            // If the expected type is a custom made context.
            if (expectedType.Namespace == null || !expectedType.Namespace.StartsWith(nameof(Disqord)))
            {
                message += " If you have not overridden the context creation methods, you must do so and have it return the given context type. " +
                    "Otherwise ensure it returns the correct context types.";
            }

            Logger.LogError(message, args.ToArray());
        }
        else
        {
            if (exceptionResult.Exception is OperationCanceledException && StoppingToken.IsCancellationRequested)
            {
                // Means the bot is stopping and any exceptions caused by cancellation we can ignore.
                return;
            }

            Logger.LogError(exceptionResult.Exception, exceptionResult.FailureReason);
        }
    }
}
