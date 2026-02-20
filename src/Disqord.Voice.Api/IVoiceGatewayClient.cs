using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.Voice.Api.Models;

namespace Disqord.Voice.Api;

public interface IVoiceGatewayClient : ILogging, IAsyncDisposable
{
    Snowflake GuildId { get; }

    Snowflake CurrentMemberId { get; }

    string SessionId { get; }

    string Token { get; }

    string Endpoint { get; }

    IVoiceGateway Gateway { get; }

    IVoiceGatewayHeartbeater Heartbeater { get; }

    IJsonSerializer Serializer { get; }

    /// <summary>
    ///     Gets the set of currently connected user IDs in the voice session.
    /// </summary>
    IReadOnlySet<Snowflake> ConnectedUserIds { get; }

    /// <summary>
    ///     Gets the sequence number of the last numbered message received from the gateway.
    ///     Used for <c>seq_ack</c> in v8 heartbeats.
    /// </summary>
    int LastSequenceNumber { get; }

    /// <summary>
    ///     Gets the maximum DAVE protocol version advertised during identification.
    /// </summary>
    int MaxDaveProtocolVersion { get; }

    /// <summary>
    ///     Suspends the gateway dispatch loop after the next session description is received,
    ///     allowing the caller to finish setup (e.g., DAVE handler initialization) before
    ///     subsequent messages are processed. Call <see cref="ResumeAfterSessionDescription"/>
    ///     to resume the loop.
    /// </summary>
    void SuspendAfterSessionDescription();

    /// <summary>
    ///     Resumes the gateway dispatch loop after it was suspended by <see cref="SuspendAfterSessionDescription"/>.
    /// </summary>
    /// <param name="daveMessageHandler"> An optional handler for DAVE protocol messages, invoked inline in the dispatch loop. </param>
    void ResumeAfterSessionDescription(Func<VoiceGatewayMessage, CancellationToken, Task>? daveMessageHandler);

    Task<ReadyJsonModel> WaitForReadyAsync(CancellationToken cancellationToken);

    Task<SessionDescriptionJsonModel> WaitForSessionDescriptionAsync(CancellationToken cancellationToken);

    Task SendAsync(VoiceGatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

    Task SendBinaryAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs this <see cref="IVoiceGatewayClient"/>.
    /// </summary>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(CancellationToken stoppingToken);
}
