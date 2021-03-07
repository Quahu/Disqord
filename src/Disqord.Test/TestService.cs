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
        {
            client.GuildAvailable += GuildAvailableAsync;
            client.Ready += ReadyAsync;
        }

        private Task GuildAvailableAsync(object sender, GuildAvailableEventArgs e)
        {
            Logger.LogInformation("guild available fired");
            return Task.CompletedTask;
        }

        private Task ReadyAsync(object sender, ReadyEventArgs e)
        {
            Logger.LogInformation("ready fired");
            return Task.CompletedTask;
        }
    }
}
