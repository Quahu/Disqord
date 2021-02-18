using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;
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

        public GatewayIntents Intents => ApiClient.Intents;

        public ICurrentUser CurrentUser { get; }

        public IReadOnlySet<Snowflake> UnavailableGuilds { get; }

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
            ApiClient = apiClient;

            ApiClient.DispatchReceived += Dispatcher.HandleDispatchAsync;
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
            ApiClient.DispatchReceived -= Dispatcher.HandleDispatchAsync;

            ApiClient.Dispose();
        }
    }
}
