using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordBotBase : DiscordClientBase
    {
        public CommandService Commands { get; }

        public IServiceProvider Services { get; }

        private readonly DiscordClientBase _client;

        protected DiscordBotBase(
            ILogger logger,
            CommandService commands,
            IServiceProvider services,
            DiscordClientBase client)
            : base(logger, client)
        {
            Commands = commands;
            Services = services;

            _client = client;

            MessageReceived += MessageReceivedAsync;

            Commands.CommandExecuted += CommandExecutedAsync;
            Commands.CommandExecutionFailed += CommandExecutionFailedAsync;

            Setup();
        }

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

        protected virtual DiscordCommandContext GetCommandContext(IGatewayUserMessage message, ITextChannel channel)
        {
            if (message.GuildId != null)
                return new DiscordGuildCommandContext(this, message, channel, Services);

            return new(this, message, Services);
        }

        private async Task MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message is not IGatewayUserMessage message)
                return;

            if (!CommandUtilities.HasPrefix(message.Content, '?', out var output))
                return;

            var context = GetCommandContext(message, e.Channel);
            var result = await Commands.ExecuteAsync(output, context).ConfigureAwait(false);
            if (result is not FailedResult failedResult)
                return;

            //// These will be handled by the ExecutionFailed event handler.
            //if (result is ExecutionFailedResult)
            //    return;

            await this.SendMessageAsync(message.ChannelId, new LocalMessageBuilder()
                .WithContent(failedResult.FailureReason)
                .Build());
        }

        private Task CommandExecutionFailedAsync(CommandExecutionFailedEventArgs e)
        {
            if (e.Context is not DiscordCommandContext context)
                return Task.CompletedTask;

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
                Logger.LogError(e.Result.Exception, e.Result.FailureReason);
            }

            return Task.CompletedTask;
        }

        private async Task CommandExecutedAsync(CommandExecutedEventArgs e)
        {
            if (e.Result is not DiscordCommandResult result)
                return;

            if (e.Context is not DiscordCommandContext context)
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

        public override Task RunAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);
    }
}