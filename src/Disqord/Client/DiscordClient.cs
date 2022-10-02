using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord;

public class DiscordClient : DiscordClientBase
{
    public DiscordClient(
        IOptions<DiscordClientConfiguration> options,
        ILogger<DiscordClient> logger,
        IRestClient restClient,
        IGatewayClient gatewayClient,
        IEnumerable<DiscordClientExtension> extensions)
        : base(logger, restClient, gatewayClient, extensions)
    { }

    /// <inheritdoc/>
    public override async Task RunAsync(CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        await GatewayClient.RunAsync(null, stoppingToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        return GatewayClient.Dispatcher.WaitUntilReadyAsync(cancellationToken);
    }
}
