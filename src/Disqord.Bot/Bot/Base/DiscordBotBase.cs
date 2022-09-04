using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Text;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qmmands;
using Qommon.Collections.ReadOnly;

namespace Disqord.Bot;

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
    public ICommandService Commands { get; }

    /// <summary>
    ///     Gets the service provider of this bot.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <inheritdoc/>
    public override CancellationToken StoppingToken
    {
        get => _client.StoppingToken;
        private protected set => throw new NotSupportedException(); // Not called.
    }

    /// <summary>
    ///     Gets the IDs of the users that own this bot.
    /// </summary>
    /// <remarks>
    ///     Returns an empty list if this was not set in the configuration
    ///     and <see cref="IsOwnerAsync"/> has not been executed yet.
    /// </remarks>
    public IReadOnlyList<Snowflake> OwnerIds { get; protected set; }

    private Snowflake? _applicationId;
    private IApplication? _currentApplication;

    private readonly bool _syncGlobalApplicationCommands;
    private readonly bool _syncGuildApplicationCommands;

    private readonly DiscordClientBase _client;
    private readonly DiscordBotMasterService? _masterService;

    private protected DiscordBotBase(
        IOptions<DiscordBotBaseConfiguration> options,
        ILogger logger,
        IServiceProvider services,
        DiscordClientBase client)
        : base(logger, client)
    {
        var configuration = options.Value;
        OwnerIds = configuration.OwnerIds?.ToReadOnlyList() ?? ReadOnlyList<Snowflake>.Empty;
        _applicationId = configuration.ApplicationId;
        _syncGlobalApplicationCommands = configuration.SyncGlobalApplicationCommands;
        _syncGuildApplicationCommands = configuration.SyncGuildApplicationCommands;
        Prefixes = services.GetRequiredService<IPrefixProvider>();
        Commands = services.GetRequiredService<ICommandService>();
        Services = services;
        _client = client;

        _masterService = services.GetService<DiscordBotMasterService>();
        if (_masterService == null)
        {
            MessageReceived += async (_, e) =>
            {
                if (e.Message is not IGatewayUserMessage)
                    return;

                await ProcessCommandsAsync(e).ConfigureAwait(false);
            };
        }

        InteractionReceived += async (_, e) =>
        {
            await ProcessCommandsAsync(e).ConfigureAwait(false);
        };
    }

    /// <inheritdoc/>
    public override Task RunAsync(CancellationToken stoppingToken)
    {
        return _client.RunAsync(stoppingToken);
    }

    /// <inheritdoc/>
    public override Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        return _client.WaitUntilReadyAsync(cancellationToken);
    }
}
