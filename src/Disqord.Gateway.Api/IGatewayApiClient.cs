using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Serialization.Json;
using Qommon.Events;

namespace Disqord.Gateway.Api;

public interface IGatewayApiClient : IApiClient
{
    /// <summary>
    ///     Gets the serializer of this client.
    /// </summary>
    IJsonSerializer Serializer { get; }

    /// <summary>
    ///     Gets the shard coordinator of this client.
    /// </summary>
    IShardCoordinator ShardCoordinator { get; }

    /// <summary>
    ///     Gets the shard factory of this client.
    /// </summary>
    IShardFactory ShardFactory { get; }

    /// <summary>
    ///     Gets the shards managed by this <see cref="IGatewayApiClient"/>.
    /// </summary>
    IReadOnlyDictionary<ShardId, IShard> Shards { get; }

    /// <summary>
    ///     Gets the stopping token passed to <see cref="RunAsync(Uri, CancellationToken)"/>.
    /// </summary>
    CancellationToken StoppingToken { get; }

    /// <summary>
    ///     Gets the event that fires when a gateway dispatch is received.
    /// </summary>
    AsynchronousEvent<GatewayDispatchReceivedEventArgs> DispatchReceivedEvent { get; }

    /// <summary>
    ///     Runs this <see cref="IGatewayApiClient"/>.
    /// </summary>
    /// <param name="uri"> The Discord gateway URI to connect to. </param>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(Uri uri, CancellationToken stoppingToken);
}
