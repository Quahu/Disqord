using System;
using System.Collections.Generic;
using System.Threading;
using Disqord.Bot.Hosting;
using Qommon.Collections;
using Disqord.Gateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qmmands;
using Qommon.Collections.ReadOnly;

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
        ///     Gets the command queue of this bot.
        /// </summary>
        public ICommandQueue Queue { get; }

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
            private protected set => throw new NotSupportedException(); // Not called.
        }

        /// <summary>
        ///     Gets the IDs of the users that own this bot.
        ///     Returns an empty list if this was not set in the configuration
        ///     and <see cref="IsOwnerAsync"/> has not been executed yet.
        /// </summary>
        public IReadOnlyList<Snowflake> OwnerIds { get; protected set; }

        private readonly DiscordClientBase _client;
        private readonly DiscordBotMasterService _masterService;

        private protected DiscordBotBase(
            IOptions<DiscordBotBaseConfiguration> options,
            ILogger logger,
            IServiceProvider services,
            DiscordClientBase client)
            : base(logger, client)
        {
            var configuration = options.Value;
            OwnerIds = configuration.OwnerIds?.ToReadOnlyList() ?? ReadOnlyList<Snowflake>.Empty;
            Prefixes = services.GetRequiredService<IPrefixProvider>();
            Queue = services.GetRequiredService<ICommandQueue>();
            Commands = services.GetRequiredService<CommandService>();
            Services = services;
            _client = client;

            _masterService = services.GetService<DiscordBotMasterService>();
            if (_masterService == null)
            {
                MessageReceived += async (_, e) =>
                {
                    if (e.Message is not IGatewayUserMessage message)
                        return;

                    await ProcessCommandsAsync(message, e.Channel).ConfigureAwait(false);
                };
            }

            Commands.CommandExecuted += CommandExecutedAsync;
            Commands.CommandExecutionFailed += CommandExecutionFailedAsync;
        }
    }
}
