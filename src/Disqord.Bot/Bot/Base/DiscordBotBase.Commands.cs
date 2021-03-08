using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase
    {
        // TODO: probably split into multiple steps
        protected virtual void Setup()
        {
            Commands.AddTypeParser(new SnowflakeTypeParser());
            Commands.AddTypeParser(new ColorTypeParser());

            try
            {
                var modules = Commands.AddModules(Assembly.GetEntryAssembly());
                Logger.LogInformation("Added {0} command modules with {1} commands.", modules.Count, modules.SelectMany(x => CommandUtilities.EnumerateAllCommands(x)).Count());
            }
            catch (CommandMappingException ex)
            {
                Logger.LogCritical(ex, "Failed to map command {0} in module {1}:", ex.Command, ex.Command.Module);
                throw;
            }
        }

        /// <summary>
        ///     Gets a <see cref="DiscordCommandContext"/> from the provided parameters.
        /// </summary>
        /// <param name="prefix"> The prefix found in the message. </param>
        /// <param name="message"> The messsage possibly containing commands. </param>
        /// <param name="channel"> The optional text channel the message was sent in. </param>
        /// <returns>
        ///     A <see cref="DiscordCommandContext"/> or a <see cref="DiscordGuildCommandContext"/> for guild messages.
        /// </returns>
        protected virtual DiscordCommandContext GetCommandContext(IPrefix prefix, IGatewayUserMessage message, ITextChannel channel)
        {
            if (message.GuildId != null)
                return new DiscordGuildCommandContext(this, prefix, message, channel, Services);

            return new(this, prefix, message, Services);
        }

        private async Task MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message is not IGatewayUserMessage message)
                return;

            IEnumerable<IPrefix> prefixes;
            try
            {
                prefixes = await Prefixes.GetPrefixesAsync(message).ConfigureAwait(false);
                if (prefixes == null)
                    return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while getting the prefixes.");
                return;
            }

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
                    return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while finding the prefixes.");
                return;
            }

            DiscordCommandContext context;
            try
            {
                context = GetCommandContext(foundPrefix, message, e.Channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while getting the command context.");
                return;
            }

            Queue.Post(output, context, static (input, context) => context.Bot.ExecuteAsync(input, context));
        }

        public async Task ExecuteAsync(string input, DiscordCommandContext context)
        {
            var result = await Commands.ExecuteAsync(input, context).ConfigureAwait(false);
            if (result is not FailedResult failedResult)
                return;

            //// These will be handled by the ExecutionFailed event handler.
            //if (result is ExecutionFailedResult)
            //    return;

            // TODO: result handling... Map of Result -> string?
            if (failedResult is CommandNotFoundResult)
                return;

            await this.SendMessageAsync(context.ChannelId, new LocalMessageBuilder()
                .WithContent(failedResult.FailureReason)
                .Build());
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
                    message += " Did you forget to mark the module with {3}?";
                    args.Add(nameof(RequireGuildAttribute));
                }

                // If the expected type is a custom made context.
                if (contextTypeMismatchException.ExpectedType != typeof(DiscordGuildCommandContext)
                    && contextTypeMismatchException.ExpectedType != typeof(DiscordCommandContext))
                {
                    message += " If you have not overridden {4} yet, you must do so and have it return the given context type. " +
                        "Otherwise ensure it returns the correct context types.";
                    args.Add(nameof(DiscordBotBase.GetCommandContext));
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

            return Task.CompletedTask;
        }

        private async Task CommandExecutedAsync(CommandExecutedEventArgs e)
        {
            if (e.Result is not DiscordCommandResult result)
                return;

            try
            {
                await result.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred when handling command result of type {0}.", result.GetType().Name);
            }
        }
    }
}