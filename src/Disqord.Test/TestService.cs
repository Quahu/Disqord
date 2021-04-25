using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
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

            while (true)
            {
                // long-running background logic here
                await Task.Delay(TimeSpan.FromMinutes(5));
                Logger.LogInformation("5 minutes passed!");
            }
        }

        protected override ValueTask OnMessageReceived(MessageReceivedEventArgs e)
        {
            Logger.LogInformation("Received message in {0}ms", (DateTimeOffset.UtcNow - e.Message.CreatedAt).TotalMilliseconds);
            return default;
        }
    }
}
