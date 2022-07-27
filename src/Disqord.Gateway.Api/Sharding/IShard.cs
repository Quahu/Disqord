using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api;

public interface IShard : ILogging, IAsyncDisposable
{
    /// <summary>
    ///     Gets the ID of this shard.
    /// </summary>
    ShardId Id { get; }

    GatewayIntents Intents { get; }

    int LargeThreshold { get; }

    UpdatePresenceJsonModel? Presence { get; set; }

    IGatewayApiClient ApiClient { get; }

    IJsonSerializer Serializer { get; }

    IGateway Gateway { get; }

    IGatewayRateLimiter RateLimiter { get; }

    IGatewayHeartbeater Heartbeater { get; }

    /// <summary>
    ///     Gets the session ID of the current gateway session.
    /// </summary>
    string? SessionId { get; }

    /// <summary>
    ///     Gets the last sequence number (<see cref="GatewayPayloadJsonModel.S"/>) received from the gateway.
    /// </summary>
    int? Sequence { get; }

    GatewayState State { get; }

    /// <summary>
    ///     Gets the stopping token passed to <see cref="RunAsync(Uri, CancellationToken)"/>.
    /// </summary>
    CancellationToken StoppingToken { get; }

    Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Waits for this shard to be ready.
    /// </summary>
    /// <remarks>
    ///     This should only be called prior to startup.
    ///     If the client is already ready this will hang until the shard identifies again.
    ///     This does not throw if the shard run fails.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task"/> representing the wait.
    /// </returns>
    Task WaitForReadyAsync();

    /// <summary>
    ///     Runs this <see cref="IGatewayApiClient"/>.
    /// </summary>
    /// <param name="uri"> The Discord gateway URI to connect to. </param>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(Uri uri, CancellationToken stoppingToken);
}
