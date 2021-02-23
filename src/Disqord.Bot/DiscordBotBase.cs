using System;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase
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
    }
}