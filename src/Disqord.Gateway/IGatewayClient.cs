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

    /// <inheritdoc cref="IGatewayDispatcher.CurrentUser"/>
    ICurrentUser CurrentUser => Dispatcher.CurrentUser;

    /// <inheritdoc cref="IGatewayDispatcher.CurrentApplicationId"/>
    Snowflake CurrentApplicationId => Dispatcher.CurrentApplicationId;

    /// <inheritdoc cref="IGatewayDispatcher.CurrentApplicationFlags"/>
    ApplicationFlags CurrentApplicationFlags => Dispatcher.CurrentApplicationFlags;

    /// <summary>
    ///     Runs this <see cref="IGatewayClient"/>.
    /// </summary>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <param name="initialUri"> The Discord gateway URI to connect to. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(Uri? initialUri, CancellationToken stoppingToken);
}
