using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;

namespace Disqord
{
    /// <summary>
    ///     Represents a high-level Discord client.
    ///     Wraps <see cref="IRestClient"/> and <see cref="IGatewayClient"/>.
    /// </summary>
    public abstract partial class DiscordClientBase : IRestClient, IGatewayClient
    {
        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the REST client this client wraps.
        /// </summary>
        public IRestClient RestClient { get; }

        /// <summary>
        ///     Gets the gateway client this client wraps.
        /// </summary>
        public IGatewayClient GatewayClient { get; }

        public ICurrentUser CurrentUser => GatewayClient.CurrentUser;

        /// <inheritdoc cref="IClient.ApiClient"/>
        public DiscordApiClient ApiClient { get; }

        /// <summary>
        ///     Gets the <see cref="CancellationToken"/> passed to <see cref="RunAsync(CancellationToken)"/>.
        ///     This is set by implementations of this type.
        ///     Returns <see cref="CancellationToken.None"/> if the client has not been started.
        /// </summary>
        public virtual CancellationToken StoppingToken { get; protected set; }

        IApiClient IClient.ApiClient => ApiClient;
        IGatewayApiClient IGatewayClient.ApiClient => GatewayClient.ApiClient;
        IRestApiClient IRestClient.ApiClient => RestClient.ApiClient;
        IGatewayCacheProvider IGatewayClient.CacheProvider => GatewayClient.CacheProvider;
        IGatewayDispatcher IGatewayClient.Dispatcher => GatewayClient.Dispatcher;
        IDictionary<Snowflake, IDirectChannel> IRestClient.DirectChannels => RestClient.DirectChannels;
        IReadOnlyDictionary<ShardId, IGatewayApiClient> IGatewayClient.Shards => GatewayClient.Shards;

        private readonly Dictionary<Type, DiscordClientExtension> _extensions;

        /// <summary>
        ///     Instantiates a new <see cref="DiscordClientBase"/>, wrapping REST and gateway clients.
        /// </summary>
        /// <param name="logger"> The logger of this client. </param>
        /// <param name="restClient"> The REST client to wrap. </param>
        /// <param name="gatewayClient"> The gateway client to wrap. </param>
        /// <param name="apiClient"> The API client of this client. </param>
        /// <param name="extensions"> The extensions to use. </param>
        protected DiscordClientBase(
            ILogger logger,
            IRestClient restClient,
            IGatewayClient gatewayClient,
            DiscordApiClient apiClient,
            IEnumerable<DiscordClientExtension> extensions)
        {
            Logger = logger;
            RestClient = restClient;
            GatewayClient = gatewayClient;
            ApiClient = apiClient;
            _extensions = extensions.ToDictionary(x => x.GetType(), x => x);

            // Binds `this` to the dispatcher, where `this` is the DiscordClientBase.
            GatewayClient.Dispatcher.Bind(this);
        }

        /// <summary>
        ///     Instantiates a new <see cref="DiscordClientBase"/>, wrapping a pre-existing client.
        ///     Rebinds the specified client's dispatcher to <see langword="this"/>.
        /// </summary>
        /// <param name="logger"> The logger of this client. </param>
        /// <param name="client"> The client to wrap. </param>
        protected DiscordClientBase(
            ILogger logger,
            DiscordClientBase client)
        {
            Logger = logger;
            RestClient = client.RestClient;
            GatewayClient = client.GatewayClient;
            ApiClient = client.ApiClient;
            _extensions = client._extensions;

            // Binds `this` to the dispatcher, where `this` is the client implementing DiscordBotBase,
            // wrapping an existing DiscordClientBase.
            client.GatewayClient.Dispatcher.Bind(this);
        }

        /// <summary>
        ///     Runs this client.
        /// </summary>
        /// <param name="stoppingToken"> The token used to signal stopping. </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the work.
        /// </returns>
        public abstract Task RunAsync(CancellationToken stoppingToken);

        /// <summary>
        ///     Waits until this client is ready, respecting the configured <see cref="ReadyEventDelayMode"/>.
        /// </summary>
        /// <param name="cancellationToken"> The token to observe for cancellation. </param>
        /// <returns>
        ///     A <see cref="Task"/> that completes when this client is ready.
        /// </returns>
        public abstract Task WaitUntilReadyAsync(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public void Dispose()
        {
            GatewayClient.Dispose();
            RestClient.Dispose();
        }

        Task IGatewayClient.RunAsync(Uri uri, CancellationToken stoppingToken)
            => GatewayClient.RunAsync(uri, stoppingToken);
    }
}
