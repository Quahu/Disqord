using System;
using System.Collections.Generic;
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
    public abstract partial class DiscordClientBase : IRestClient, IGatewayClient
    {
        public ILogger Logger { get; }

        public IRestClient RestClient { get; }

        public IGatewayClient GatewayClient { get; }

        public ICurrentUser CurrentUser => GatewayClient.CurrentUser;

        public IReadOnlySet<Snowflake> UnavailableGuilds => GatewayClient.UnavailableGuilds;

        public DiscordApiClient ApiClient { get; }

        IApiClient IClient.ApiClient => ApiClient;
        IGatewayApiClient IGatewayClient.ApiClient => GatewayClient.ApiClient;
        IRestApiClient IRestClient.ApiClient => RestClient.ApiClient;
        IGatewayCacheProvider IGatewayClient.CacheProvider => GatewayClient.CacheProvider;
        IGatewayDispatcher IGatewayClient.Dispatcher => GatewayClient.Dispatcher;

        protected DiscordClientBase(
            ILogger logger,
            IRestClient restClient,
            IGatewayClient gatewayClient,
            DiscordApiClient apiClient)
        {
            Logger = logger;
            RestClient = restClient;
            GatewayClient = gatewayClient;
            ApiClient = apiClient;
        }

        protected DiscordClientBase(
            ILogger logger,
            DiscordClientBase client)
        {
            Logger = logger;
            RestClient = client.RestClient;
            GatewayClient = client.GatewayClient;
            ApiClient = client.ApiClient;

            // Binds `this` to the dispatcher, where `this` is the client implementing DiscordBotBase.
            client.GatewayClient.Dispatcher.Bind(this);
        }

        public abstract Task RunAsync(CancellationToken stoppingToken);

        public void Dispose()
        {
            GatewayClient.Dispose();
            RestClient.Dispose();
        }

        Task IGatewayClient.RunAsync(Uri uri, CancellationToken stoppingToken)
            => GatewayClient.RunAsync(uri, stoppingToken);
    }
}
