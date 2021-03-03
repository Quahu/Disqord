using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayClient : IGatewayClient
    {
        public ILogger Logger { get; }

        public IGatewayCacheProvider CacheProvider { get; }

        public IGatewayDispatcher Dispatcher { get; }

        public IGatewayApiClient ApiClient { get; }

        public IReadOnlyDictionary<ShardId, IGatewayApiClient> Shards { get; }

        private bool _isDisposed;

        public DefaultGatewayClient(
            IOptions<DefaultGatewayClientConfiguration> options,
            ILogger<DefaultGatewayClient> logger,
            IGatewayCacheProvider cacheProvider,
            IGatewayDispatcher dispatcher,
            IGatewayApiClient apiClient)
        {
            Logger = logger;
            CacheProvider = cacheProvider;
            CacheProvider.Bind(this);
            Dispatcher = dispatcher;
            Dispatcher.Bind(this);

            if (apiClient != null)
            {
                ApiClient = apiClient;
                Shards = new Dictionary<ShardId, IGatewayApiClient>(1)
                {
                    [ShardId.Default] = ApiClient
                }.ReadOnly();

                ApiClient.DispatchReceived += Dispatcher.HandleDispatchAsync;
            }
            else
            {
                Shards = new SynchronizedDictionary<ShardId, IGatewayApiClient>();
            }
        }

        public DefaultGatewayClient(
            IOptions<DefaultGatewayClientConfiguration> options,
            ILogger<DefaultGatewayClient> logger,
            IGatewayCacheProvider cacheProvider,
            IGatewayDispatcher dispatcher)
            : this(options, logger, cacheProvider, dispatcher, null)
        {
            // This is the constructor DiscordClientSharder uses.
        }

        public Task RunAsync(Uri uri, CancellationToken cancellationToken)
        {
            return ApiClient.RunAsync(uri, cancellationToken);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // ApiClient will be null if this is managed by DiscordClientSharder.
            if (ApiClient != null)
            {
                ApiClient.DispatchReceived -= Dispatcher.HandleDispatchAsync;
                ApiClient.Dispose();
            }
        }
    }
}
