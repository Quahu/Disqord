using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Gateway.Default;
using Disqord.Gateway.Default.Dispatcher;
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
        var uri = new Uri(Discord.Gateway.Url);
        await GatewayClient.RunAsync(uri, stoppingToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        if (GatewayClient.Dispatcher is DefaultGatewayDispatcher dispatcher && dispatcher["READY"] is ReadyDispatchHandler readyHandler)
            return Task.WhenAll(readyHandler.InitialReadys.Values.Select(tcs => tcs.Task));

        return Task.CompletedTask;
    }
}
