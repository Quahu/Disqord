using System.Threading;
using System.Threading.Tasks;
using Disqord.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Test
{
    public class TestService : DiscordClientService
    {
        public TestService(
            ILogger<TestService> logger,
            DiscordClientBase client)
            : base(logger, client)
        { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitUntilReadyAsync(stoppingToken);
            Logger.LogInformation("Client says it's ready which is really cool.");

            // write cache-dependent code here
        }
    }
}
