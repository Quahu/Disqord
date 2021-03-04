using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents a <see cref="DiscordClientBase"/> with additional command processing capabilities.
    /// </summary>
    public abstract partial class DiscordBotBase : DiscordClientBase
    {
        /// <summary>
        ///     Gets the prefix provider of this bot.
        /// </summary>
        public IPrefixProvider Prefixes { get; }

        /// <summary>
        ///     Gets the command service of this bot.
        /// </summary>
        public CommandService Commands { get; }

        /// <summary>
        ///     Gets the service provider of this bot.
        /// </summary>
        public IServiceProvider Services { get; }

        public override CancellationToken StoppingToken
        {
            get => _client.StoppingToken;
            protected set => throw new NotSupportedException(); // Not called.
        }

        private readonly DiscordClientBase _client;

        protected DiscordBotBase(
            ILogger logger,
            IPrefixProvider prefixes,
            CommandService commands,
            IServiceProvider services,
            DiscordClientBase client)
            : base(logger, client)
        {
            Prefixes = prefixes;
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