using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase
    {
        /// <inheritdoc/>
        public override Task RunAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);

        /// <inheritdoc/>
        public override Task WaitUntilReadyAsync(CancellationToken cancellationToken)
            => _client.WaitUntilReadyAsync(cancellationToken);
    }
}
