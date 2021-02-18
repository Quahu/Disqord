using System;
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

        protected virtual DiscordCommandContext GetCommandContext(IUserMessage message)
            => new(this, message, Services);

        private async Task MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message is not IUserMessage message)
                return;

            if (!CommandUtilities.HasPrefix(message.Content, '?', out var output))
                return;

            var context = GetCommandContext(message);
            var result = await Commands.ExecuteAsync(output, context).ConfigureAwait(false);
            if (result is not FailedResult failedResult)
                return;

            await this.SendMessageAsync(message.ChannelId, new LocalMessageBuilder()
                .WithContent(failedResult.FailureReason)
                .Build());
        }

        private Task CommandExecutedAsync(CommandExecutedEventArgs e)
        {
            if (e.Result is not DiscordCommandResult result)
                return Task.CompletedTask;

            if (e.Context is not DiscordCommandContext context)
                return Task.CompletedTask;

            return result.ExecuteAsync(context);
        }

        public override Task RunAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);
    }
}