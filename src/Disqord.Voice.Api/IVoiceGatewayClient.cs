using System;
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

    Task<ReadyJsonModel> WaitForReadyAsync(CancellationToken cancellationToken);

    Task<SessionDescriptionJsonModel> WaitForSessionDescriptionAsync(CancellationToken cancellationToken);

    Task SendAsync(VoiceGatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Runs this <see cref="IVoiceGatewayClient"/>.
    /// </summary>
    /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
    /// <returns> The <see cref="Task"/> representing the connection. </returns>
    Task RunAsync(CancellationToken stoppingToken);
}
