using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase
    {
        public override Task RunAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);
    }
}