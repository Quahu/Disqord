using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord
{
    public class DiscordApiClient : IRestApiClient, IGatewayApiClient
    {
        public Token Token { get; }

        public ILogger Logger { get; }

        public IRestApiClient RestApiClient { get; }

        public IGatewayApiClient GatewayApiClient { get; }

        IRestRateLimiter IRestApiClient.RateLimiter => RestApiClient.RateLimiter;
        IRestRequester IRestApiClient.Requester => RestApiClient.Requester;
        IJsonSerializer IRestApiClient.Serializer => RestApiClient.Serializer;

        IJsonSerializer IGatewayApiClient.Serializer => GatewayApiClient.Serializer;
        IGateway IGatewayApiClient.Gateway => GatewayApiClient.Gateway;
        IGatewayRateLimiter IGatewayApiClient.RateLimiter => GatewayApiClient.RateLimiter;
        IGatewayHeartbeater IGatewayApiClient.Heartbeater => GatewayApiClient.Heartbeater;
        GatewayIntents IGatewayApiClient.Intents => GatewayApiClient.Intents;
        string IGatewayApiClient.SessionId => GatewayApiClient.SessionId;
        int? IGatewayApiClient.Sequence => GatewayApiClient.Sequence;

        public DiscordApiClient(
            IOptions<DiscordApiClientConfiguration> options,
            ILogger<DiscordApiClient> logger,
            Token token,
            IRestApiClient restApiClient,
            IGatewayApiClient gatewayApiClient)
        {
            Logger = logger;
            Token = token;

            if (token != restApiClient.Token)
                throw new ArgumentException("The token must match the token of the REST API client.");

            RestApiClient = restApiClient;

            if (token != gatewayApiClient.Token)
                throw new ArgumentException("The token must match the token of the gateway API client.");

            GatewayApiClient = gatewayApiClient;
        }

        public void Dispose()
        {
            RestApiClient.Dispose();
            GatewayApiClient.Dispose();
        }

        Task IRestApiClient.ExecuteAsync(FormattedRoute route, IRestRequestContent content, IRestRequestOptions options)
            => RestApiClient.ExecuteAsync(route, content, options);

        Task<TModel> IRestApiClient.ExecuteAsync<TModel>(FormattedRoute route, IRestRequestContent content, IRestRequestOptions options)
            => RestApiClient.ExecuteAsync<TModel>(route, content, options);

        Task IGatewayApiClient.SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken)
            => GatewayApiClient.SendAsync(payload, cancellationToken);

        Task IGatewayApiClient.RunAsync(Uri uri, CancellationToken stoppingToken)
            => GatewayApiClient.RunAsync(uri, stoppingToken);

        event AsynchronousEventHandler<GatewayDispatchReceivedEventArgs> IGatewayApiClient.DispatchReceived
        {
            add => GatewayApiClient.DispatchReceived += value;
            remove => GatewayApiClient.DispatchReceived -= value;
        }
    }
}
