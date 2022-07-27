using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway.Api;

namespace Disqord.Gateway;

public partial interface IGatewayClient : IClient
{
    /// <summary>
    ///     Gets the low-level version of this client.
    /// </summary>
    new IGatewayApiClient ApiClient { get; }

    IGatewayCacheProvider CacheProvider { get; }

    IGatewayChunker Chunker { get; }

    IGatewayDispatcher Dispatcher { get; }

    IApiClient IClient.ApiClient => ApiClient;

    ICurrentUser? CurrentUser => Dispatcher.CurrentUser;

    /// <summary>
    ///     Runs this <see cref="IGatewayClient"/>.
    /// </summary>
    /// <param name="uri"> The Discord gateway URI to connect to. </param>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(Uri uri, CancellationToken stoppingToken);
}
