using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.Utilities.Binding;
using Disqord.WebSocket;

namespace Disqord.Gateway.Api
{
    public interface IGateway : IBindable<IGatewayApiClient>, ILogging, IDisposable
    {
        int Version { get; }

        IGatewayApiClient Client { get; }

        IJsonSerializer Serializer { get; }

        Func<IWebSocketClient> WebSocketClientFactory { get; }

        Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default);

        Task CloseAsync(int closeStatus, string closeMessage = null, CancellationToken cancellationToken = default);

        Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default);

        ValueTask<GatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken = default);
    }
}
