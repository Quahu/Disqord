using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Collections;
using Qommon.Collections.Synchronized;
using Disqord.Gateway.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayClient : IGatewayClient
    {
        public ILogger Logger { get; }

        public IGatewayCacheProvider CacheProvider { get; }

        public IGatewayChunker Chunker { get; }

        public IGatewayDispatcher Dispatcher { get; }

        public IReadOnlyDictionary<ShardId, IGatewayApiClient> Shards { get; }

        private readonly IGatewayApiClient _apiClient;

        public DefaultGatewayClient(
            IOptions<DefaultGatewayClientConfiguration> options,
            ILogger<DefaultGatewayClient> logger,
            IGatewayCacheProvider cacheProvider,
            IGatewayChunker chunker,
            IGatewayDispatcher dispatcher,
            IGatewayApiClient apiClient)
        {
            Logger = logger;
            CacheProvider = cacheProvider;
            CacheProvider.Bind(this);
            Chunker = chunker;
            Chunker.Bind(this);
            Dispatcher = dispatcher;

            if (apiClient != null)
            {
                _apiClient = apiClient;
                Shards = new Dictionary<ShardId, IGatewayApiClient>(1)
                {
                    [new ShardId(0, 1)] = apiClient
                }.ReadOnly();

                apiClient.DispatchReceived += Dispatcher.HandleDispatchAsync;
            }
            else
            {
                Shards = new SynchronizedDictionary<ShardId, IGatewayApiClient>();
            }

            Dispatcher.Bind(this);
        }

        public DefaultGatewayClient(
            IOptions<DefaultGatewayClientConfiguration> options,
            ILogger<DefaultGatewayClient> logger,
            IGatewayCacheProvider cacheProvider,
            IGatewayChunker chunker,
            IGatewayDispatcher dispatcher)
            : this(options, logger, cacheProvider, chunker, dispatcher, null)
        {
            // This is the constructor DiscordClientSharder uses.
        }

        public Task RunAsync(Uri uri, CancellationToken cancellationToken)
            => _apiClient?.RunAsync(uri, cancellationToken) ?? throw new InvalidOperationException();
    }
}
