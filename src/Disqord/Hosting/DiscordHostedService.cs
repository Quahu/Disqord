using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Hosting
{
    public class DiscordHostedService : BackgroundService, ILogging
    {
        public ILogger Logger { get; }

        public DiscordClientBase Client { get; }

        public DiscordHostedService(
            ILogger<DiscordHostedService> logger,
            DiscordClientBase client)
        {
            Logger = logger;
            Client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Logger.LogDebug("Hosting the Discord client of type {0}.", Client.GetType().Name);
                await Client.RunAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (WebSocketClosedException)
            {
                Logger.LogCritical("Hosting of the Discord client was interrupted due to the unrecoverable closure. Take appropriate action in order to resolve the issue.");
            }
        }
    }
}
