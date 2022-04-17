using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Hosting
{
    /// <summary>
    ///     Represents an <see cref="IHostedService"/> that sets up the specified <see cref="DiscordClientBase"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DiscordClientSetupService : IHostedService, ILogging
    {
        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the set up client.
        /// </summary>
        public DiscordClientBase Client { get; }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordClientSetupService"/>.
        /// </summary>
        /// <param name="logger"> The logger. </param>
        /// <param name="client"> The client to set up. </param>
        public DiscordClientSetupService(
            ILogger<DiscordClientSetupService> logger,
            DiscordClientBase client)
        {
            Logger = logger;
            Client = client;
        }

        /// <inheritdoc/>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            return Client.InitializeExtensionsAsync(cancellationToken);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
