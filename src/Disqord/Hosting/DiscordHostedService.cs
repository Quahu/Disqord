using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Hosting
{
    /// <summary>
    ///     Represents a <see cref="BackgroundService"/> that runs the specified <see cref="DiscordClientBase"/>.
    /// </summary>
    public class DiscordHostedService : BackgroundService, ILogging
    {
        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the hosted client.
        /// </summary>
        public DiscordClientBase Client { get; }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordHostedService"/>.
        /// </summary>
        /// <param name="logger"> The logger. </param>
        /// <param name="client"> The client to host. </param>
        public DiscordHostedService(
            ILogger<DiscordHostedService> logger,
            DiscordClientBase client)
        {
            Logger = logger;
            Client = client;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Logger.LogDebug("Hosting the Discord client of type {0}.", Client.GetType().Name);
                await Client.RunAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (WebSocketClosedException)
            {
                Logger.LogCritical("Hosting of the Discord client was interrupted due to an unrecoverable error. Take appropriate actions to resolve the issue.");
            }
        }
    }
}
