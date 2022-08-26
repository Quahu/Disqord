using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Serialization.Json;
using Qommon.Events;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents a low-level client for the Discord gateway API.
/// </summary>
/// <remarks>
///     <inheritdoc/>
/// </remarks>
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
    /// <remarks>
    ///     This is only populated after the client is running,
    ///     i.e. after the shard set is retrieved from the shard coordinator.
    ///     <br/>
    ///     In a multi-process sharding setup this represents only the shards handled by this client.
    /// </remarks>
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
    ///     Runs this API client which creates and runs the shards
    ///     returned from the coordinator.
    /// </summary>
    /// <param name="initialUri"> The initial URI of the Discord gateway to connect the shards to. </param>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns>
    ///     The <see cref="Task"/> representing the run.
    /// </returns>
    Task RunAsync(Uri? initialUri, CancellationToken stoppingToken);
}
