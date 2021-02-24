using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase
    {
        /// <inheritdoc/>
        public override Task RunAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);
    }
}