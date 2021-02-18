using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Disqord.WebSocket;
using Disqord.WebSocket.Default;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Api.Default
{
    public class DefaultGateway : IGateway
    {
        public int Version { get; }

        public bool LogsPayloads { get; }

        public bool UsesZLib { get; }

        public ILogger Logger { get; }

        public IJsonSerializer Serializer { get; }

        public Func<IWebSocketClient> WebSocketClientFactory { get; }

        private readonly DiscordWebSocket _ws;

        public DefaultGateway(
            IOptions<DefaultGatewayConfiguration> options,
            ILogger<DefaultGateway> logger,
            IJsonSerializer serializer)
        {
            var configuration = options.Value;
            Version = configuration.Version;
            LogsPayloads = configuration.LogsPayloads;
            UsesZLib = configuration.UsesZLib;
            Logger = logger;
            Serializer = serializer;
            WebSocketClientFactory = () => new DefaultWebSocketClient();

            _ws = new DiscordWebSocket(this, WebSocketClientFactory);
        }

        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            var uriBuilder = new UriBuilder(uri)
            {
                Query = $"v={Version}&encoding=json"
            };

            if (UsesZLib)
                uriBuilder.Query += "&compress=zlib-stream";

            uri = uriBuilder.Uri;
            Logger.LogDebug("Connecting to {0}", uri);
            return _ws.ConnectAsync(uri, cancellationToken);
        }

        public Task CloseAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Closing with status {0}: {1}", closeStatus, closeMessage);
            return _ws.CloseAsync(closeStatus, closeMessage, cancellationToken);
        }

        public Task SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
        {
            var json = Serializer.Serialize(payload);
            if (LogsPayloads)
                Logger.LogTrace("Sending payload: {0}", Encoding.UTF8.GetString(json.Span));

            if (json.Length > 4096)
                throw new ArgumentException("Cannot send payloads longer than 4096 bytes.", nameof(payload));

            return _ws.SendAsync(json, cancellationToken);
        }

        public async ValueTask<GatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            // TODO: optimise the streams
            var jsonStream = await _ws.ReceiveAsync(cancellationToken).ConfigureAwait(false);
            if (jsonStream is not MemoryStream memoryStream || !memoryStream.TryGetBuffer(out var json))
            {
                memoryStream = new MemoryStream();
                jsonStream.CopyTo(memoryStream);
                memoryStream.TryGetBuffer(out json);
            }

            if (LogsPayloads)
                Logger.LogTrace("Received payload: {0}", Encoding.UTF8.GetString(json));

            return Serializer.Deserialize<GatewayPayloadJsonModel>(json);
        }

        public void Dispose()
        {
            _ws.Dispose();
        }
    }
}
