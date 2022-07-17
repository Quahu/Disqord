using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.WebSocket;
using Qommon.Binding;

namespace Disqord.Gateway.Api;

public interface IGateway : IBindable<IGatewayApiClient>, ILogging
{
    int Version { get; }

    IGatewayApiClient Client { get; }

    IJsonSerializer Serializer { get; }

    IWebSocketClientFactory WebSocketClientFactory { get; }

    ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken);

    ValueTask CloseAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken);

    ValueTask SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken);

    ValueTask<GatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken);
}