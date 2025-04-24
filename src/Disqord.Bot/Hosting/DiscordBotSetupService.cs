using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot.Hosting;

/// <summary>
///     Represents an <see cref="IHostedService"/> that sets up the specified <see cref="DiscordBotBase"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class DiscordBotSetupService : IHostedService, ILogging
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <summary>
    ///     Gets the set-up client.
    /// </summary>
    public DiscordBotBase Bot { get; }

    /// <summary>
    ///     Instantiates a new <see cref="DiscordBotSetupService"/>.
    /// </summary>
    /// <param name="logger"> The logger. </param>
    /// <param name="bot"> The bot to set up. </param>
    public DiscordBotSetupService(
        ILogger<DiscordBotSetupService> logger,
        DiscordBotBase bot)
    {
        Logger = logger;
        Bot = bot;
    }

    /// <inheritdoc/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Bot.InitializeAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogCritical(ex, "An exception occurred while initializing the bot.");
            throw;
        }
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
