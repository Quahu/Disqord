using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Logging;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Qommon.Events;

namespace Disqord.Api;

/// <inheritdoc cref="IApiClient"/>
public class DiscordApiClient : IRestApiClient, IGatewayApiClient
{
    /// <summary>
    ///     Gets the JSON serializer of this client.
    /// </summary>
    public IJsonSerializer Serializer => _client.RestClient.ApiClient.Serializer;

    /// <inheritdoc/>
    public IReadOnlyDictionary<ShardId, IShard> Shards => _client.GatewayClient.ApiClient.Shards;

    ILogger ILogging.Logger => _client.Logger;

    Token IApiClient.Token => _client.RestClient.ApiClient.Token;

    IRestRateLimiter IRestApiClient.RateLimiter => _client.RestClient.ApiClient.RateLimiter;

    IRestRequester IRestApiClient.Requester => _client.RestClient.ApiClient.Requester;

    IShardCoordinator IGatewayApiClient.ShardCoordinator => _client.GatewayClient.ApiClient.ShardCoordinator;

    IShardFactory IGatewayApiClient.ShardFactory => _client.GatewayClient.ApiClient.ShardFactory;

    CancellationToken IGatewayApiClient.StoppingToken => _client.GatewayClient.ApiClient.StoppingToken;

    AsynchronousEvent<GatewayDispatchReceivedEventArgs> IGatewayApiClient.DispatchReceivedEvent => _client.GatewayClient.ApiClient.DispatchReceivedEvent;

    private readonly DiscordClientBase _client;

    internal DiscordApiClient(
        DiscordClientBase client)
    {
        _client = client;
    }

    Task IRestApiClient.ExecuteAsync(IFormattedRoute route, IRestRequestContent? content,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        return _client.RestClient.ApiClient.ExecuteAsync(route, content, options, cancellationToken);
    }

    Task<TModel> IRestApiClient.ExecuteAsync<TModel>(IFormattedRoute route, IRestRequestContent? content,
        IRestRequestOptions? options, CancellationToken cancellationToken)
        where TModel : class
    {
        return _client.RestClient.ApiClient.ExecuteAsync<TModel>(route, content, options, cancellationToken);
    }

    Task IGatewayApiClient.RunAsync(Uri? initialUri, CancellationToken stoppingToken)
    {
        return _client.GatewayClient.ApiClient.RunAsync(initialUri, stoppingToken);
    }
}
