using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.WebSocket;
using Qommon.Binding;

namespace Disqord.Gateway.Api;

public interface IGateway : ILogging, IAsyncDisposable, IBindable<IShard>
{
    int Version { get; }

    /// <summary>
    ///     Gets the shard of this gateway.
    /// </summary>
    IShard Shard { get; }

    IJsonSerializer Serializer { get; }

    IWebSocketClientFactory WebSocketClientFactory { get; }

    ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken);

    ValueTask CloseAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken);

    ValueTask SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken);

    ValueTask<GatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken);
}
