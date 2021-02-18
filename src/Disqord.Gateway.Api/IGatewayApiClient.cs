using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Events;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api
{
    public interface IGatewayApiClient : IApiClient
    {
        IJsonSerializer Serializer { get; }

        IGateway Gateway { get; }

        IGatewayRateLimiter RateLimiter { get; }

        IGatewayHeartbeater Heartbeater { get; }

        GatewayIntents Intents { get; }

        /// <summary>
        ///     Gets the session ID of the current gateway session.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        ///     Gets the last sequence number (<see cref="GatewayPayloadJsonModel.S"/>) received from the gateway.
        /// </summary>
        int? Sequence { get; }

        /// <summary>
        ///     Fires when a gateway dispatch is received.
        /// </summary>
        event AsynchronousEventHandler<GatewayDispatchReceivedEventArgs> DispatchReceived;

        Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Runs this <see cref="IGatewayApiClient"/>.
        /// </summary>
        /// <param name="uri"> The Discord gateway URI to connect to. </param>
        /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
        /// <returns> The <see cref="Task"/> representing the connection. </returns>
        Task RunAsync(Uri uri, CancellationToken stoppingToken);
    }
}
