using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayClient : IGatewayClient
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public IGatewayApiClient ApiClient { get; }

    /// <inheritdoc/>
    public IGatewayCacheProvider CacheProvider { get; }

    /// <inheritdoc/>
    public IGatewayChunker Chunker { get; }

    /// <inheritdoc/>
    public IGatewayDispatcher Dispatcher { get; }

    public DefaultGatewayClient(
        IOptions<DefaultGatewayClientConfiguration> options,
        ILogger<DefaultGatewayClient> logger,
        IGatewayApiClient apiClient,
        IGatewayCacheProvider cacheProvider,
        IGatewayChunker chunker,
        IGatewayDispatcher dispatcher)
    {
        Logger = logger;
        ApiClient = apiClient;
        CacheProvider = cacheProvider;
        CacheProvider.Bind(this);
        Chunker = chunker;
        Chunker.Bind(this);
        Dispatcher = dispatcher;
        Dispatcher.Bind(this);
    }

    /// <inheritdoc/>
    public Task RunAsync(Uri? initialUri, CancellationToken stoppingToken)
    {
        return ApiClient.RunAsync(initialUri, stoppingToken);
    }
}
