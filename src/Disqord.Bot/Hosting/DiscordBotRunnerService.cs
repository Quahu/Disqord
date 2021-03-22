using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot.Hosting
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DiscordBotRunnerService : DiscordClientRunnerService
    {
        public DiscordBotRunnerService(
            ILogger<DiscordBotRunnerService> logger,
            DiscordBotBase bot)
            : base(logger, bot)
        { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var bot = Client as DiscordBotBase;
            await bot.SetupAsync(cancellationToken).ConfigureAwait(false);
            await base.StartAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
