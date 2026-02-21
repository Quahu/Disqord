using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.Voice.Api.Models;
using Disqord.WebSocket;
using Qommon.Binding;

namespace Disqord.Voice.Api;

public interface IVoiceGateway : IBindable<IVoiceGatewayClient>, ILogging, IAsyncDisposable
{
    int Version { get; }

    IVoiceGatewayClient Client { get; }

    IJsonSerializer Serializer { get; }

    IWebSocketClientFactory WebSocketClientFactory { get; }

    ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken = default);

    ValueTask CloseAsync(int closeStatus, string? closeMessage = null, CancellationToken cancellationToken = default);

    ValueTask SendAsync(VoiceGatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

    ValueTask SendBinaryAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);

    ValueTask<VoiceGatewayMessage> ReceiveAsync(CancellationToken cancellationToken = default);
}
