using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord
{
    public class DiscordClient : DiscordClientBase
    {
        public DiscordClient(
            IOptions<DiscordClientConfiguration> options,
            ILogger<DiscordClient> logger,
            IRestClient restClient,
            IGatewayClient gatewayClient,
            DiscordApiClient apiClient,
            IEnumerable<DiscordClientExtension> extensions)
            : base(logger, restClient, gatewayClient, apiClient, extensions)
        {

        }

        /// <inheritdoc/>
        public override async Task RunAsync(CancellationToken stoppingToken)
        {
            Uri uri;
            if (ApiClient.Token is BotToken)
            {
                uri = new Uri("wss://gateway.discord.gg/");
            }
            else
            {
                uri = new Uri("wss://gateway.discord.gg/");
            }

            await GatewayClient.RunAsync(uri, stoppingToken).ConfigureAwait(false);
        }
    }
}
