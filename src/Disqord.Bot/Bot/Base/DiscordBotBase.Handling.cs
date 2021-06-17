using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase
    {
        internal async ValueTask<bool> ProcessCommandsAsync(IGatewayUserMessage message, CachedTextChannel channel)
        {
            // We check if the message is suitable for execution.
            // By default excludes bot messages.
            try
            {
                if (!await CheckMessageAsync(message).ConfigureAwait(false))
                    return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while executing the check message callback.");
                return false;
            }

            // We get the prefixes from the prefix provider.
            IEnumerable<IPrefix> prefixes;
            try
            {
                prefixes = await Prefixes.GetPrefixesAsync(message).ConfigureAwait(false);
                if (prefixes == null)
                    return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while getting the prefixes.");
                return false;
            }

            // We try to find a prefix in the message.
            IPrefix foundPrefix = null;
            string output = null;
            try
            {
                foreach (var prefix in prefixes)
                {
                    if (prefix == null)
                        continue;

                    if (prefix.TryFind(message, out output))
                    {
                        foundPrefix = prefix;
                        break;
                    }
                }

                if (foundPrefix == null)
                    return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while finding the prefixes in the message.");
                return false;
            }

            // We create a command context for Qmmands.
            DiscordCommandContext context;
            try
            {
                context = CreateCommandContext(foundPrefix, output, message, channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while creating the command context.");
                return false;
            }

            // We check the before execution callback, by default returns true.
            try
            {
                if (!await BeforeExecutedAsync(context).ConfigureAwait(false))
                    return false;
            }
            catch (Exception ex)
            {
                await DisposeContextAsync(context).ConfigureAwait(false);
                Logger.LogError(ex, "An exception occurred while executing the before executed callback.");
                return false;
            }

            // We post the execution to the command queue.
            // See the Post() method in the default queue for more information.
            try
            {
                Queue.Post(context, context => context.Bot.ExecuteAsync(context));
                return true;
            }
            catch (Exception ex)
            {
                await DisposeContextAsync(context).ConfigureAwait(false);
                Logger.LogError(ex, "An exception occurred while posting the execution to the command queue.");
                return false;
            }
        }

        public async Task ExecuteAsync(DiscordCommandContext context)
        {
            var result = await Commands.ExecuteAsync(context.Input, context).ConfigureAwait(false);
            if (result is not FailedResult failedResult)
                return;

            // These will be handled by the CommandExecutionFailed event handler.
            if (result is CommandExecutionFailedResult)
                return;

            await InternalHandleFailedResultAsync(context, failedResult).ConfigureAwait(false);
        }

        private async Task DisposeContextAsync(DiscordCommandContext context)
        {
            try
            {
                await context.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while disposing of the command context.");
            }
        }

        private async Task InternalHandleFailedResultAsync(DiscordCommandContext context, FailedResult result)
        {
            try
            {
                await HandleFailedResultAsync(context, result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while handling the failed result of type {0}.", result.GetType().Name);
            }

            if (result is CommandNotFoundResult && _masterService != null)
                await _masterService.HandleCommandNotFound(context).ConfigureAwait(false);

            await DisposeContextAsync(context).ConfigureAwait(false);
        }

        private async Task CommandExecutedAsync(CommandExecutedEventArgs e)
        {
            if (e.Result is not DiscordCommandResult result)
                return;

            if (e.Context is not DiscordCommandContext context)
                return;

            try
            {
                await using (RuntimeDisposal.WrapAsync(result))
                {
                    await result.ExecuteAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred when handling command result of type {0}.", result.GetType().Name);
            }

            await DisposeContextAsync(context).ConfigureAwait(false);
        }

        private Task CommandExecutionFailedAsync(CommandExecutionFailedEventArgs e)
        {
            if (e.Result.CommandExecutionStep == CommandExecutionStep.Command && e.Result.Exception is ContextTypeMismatchException contextTypeMismatchException)
            {
                var message = "A command context type mismatch occurred while attempting to execute {0}. " +
                    "The module expected {1}, but got {2}.";
                var args = new List<object>(5)
                {
                    e.Result.Command.Name,
                    contextTypeMismatchException.ExpectedType,
                    contextTypeMismatchException.ActualType
                };

                // If the expected type is a DiscordGuildCommandContext, the actual type is a DiscordCommandContext, and the module doesn't have guild restrictions.
                if (typeof(DiscordGuildCommandContext).IsAssignableFrom(contextTypeMismatchException.ExpectedType)
                    && typeof(DiscordCommandContext).IsAssignableFrom(contextTypeMismatchException.ActualType)
                    && !CommandUtilities.EnumerateAllChecks(e.Result.Command.Module).Any(x => x is RequireGuildAttribute))
                {
                    message += " Did you forget to decorate the module with {3}?";
                    args.Add(nameof(RequireGuildAttribute));
                }

                // If the expected type is a custom made context.
                if (contextTypeMismatchException.ExpectedType != typeof(DiscordGuildCommandContext)
                    && contextTypeMismatchException.ExpectedType != typeof(DiscordCommandContext))
                {
                    message += " If you have not overridden {4}, you must do so and have it return the given context type. " +
                        "Otherwise ensure it returns the correct context types.";
                    args.Add(nameof(CreateCommandContext));
                }

                Logger.LogError(message, args.ToArray());
            }
            else
            {
                if (e.Result.Exception is OperationCanceledException && StoppingToken.IsCancellationRequested)
                {
                    // Means the bot is stopping and any exceptions caused by cancellation we can ignore.
                    return Task.CompletedTask;
                }

                Logger.LogError(e.Result.Exception, e.Result.FailureReason);
            }

            if (e.Context is not DiscordCommandContext context)
                return Task.CompletedTask;

            return InternalHandleFailedResultAsync(context, e.Result);
        }
    }
}
