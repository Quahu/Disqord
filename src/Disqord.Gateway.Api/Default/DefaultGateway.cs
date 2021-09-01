using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Disqord.Utilities.Binding;
using Disqord.WebSocket;
using Disqord.WebSocket.Default.Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Api.Default
{
    public class DefaultGateway : IGateway
    {
        public int Version { get; }

        public bool LogsPayloads { get; }

        public bool UsesZLib { get; }

        public IGatewayApiClient Client => _binder.Value;

        public ILogger Logger => Client.Logger;

        public IJsonSerializer Serializer { get; }

        public IWebSocketClientFactory WebSocketClientFactory { get; }

        private DiscordWebSocket _ws;
        private readonly Binder<IGatewayApiClient> _binder;

        public DefaultGateway(
            IOptions<DefaultGatewayConfiguration> options,
            IJsonSerializer serializer,
            IWebSocketClientFactory webSocketClientFactory)
        {
            var configuration = options.Value;
            Version = configuration.Version;
            LogsPayloads = configuration.LogsPayloads;
            UsesZLib = configuration.UsesZLib;
            Serializer = serializer;
            WebSocketClientFactory = webSocketClientFactory;

            _binder = new(this);
        }

        public void Bind(IGatewayApiClient value)
        {
            _binder.Bind(value);

            _ws = new DiscordWebSocket(Logger, WebSocketClientFactory, UsesZLib);
        }

        public ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken)
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

        public ValueTask CloseAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken)
        {
            Logger.LogDebug("Closing with status {0}: {1}", closeStatus, closeMessage);
            return _ws.CloseAsync(closeStatus, closeMessage, cancellationToken);
        }

        public ValueTask SendAsync(GatewayPayloadJsonModel payload, CancellationToken cancellationToken)
        {
            var json = Serializer.Serialize(payload);
            if (LogsPayloads)
                Logger.LogTrace("Sending payload: {0}", Encoding.UTF8.GetString(json.Span));

            if (json.Length > 4096)
                throw new ArgumentException("Cannot send payloads longer than 4096 bytes.", nameof(payload));

            return _ws.SendAsync(json, cancellationToken);
        }

        public async ValueTask<GatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken)
        {
            var jsonStream = await _ws.ReceiveAsync(cancellationToken).ConfigureAwait(false);
            if (LogsPayloads)
            {
                var stream = new MemoryStream();
                await jsonStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
                Logger.LogTrace("Received payload: {0}", Encoding.UTF8.GetString(stream.GetBuffer()));
                stream.Position = 0;
                jsonStream = stream;
            }

            return Serializer.Deserialize<GatewayPayloadJsonModel>(jsonStream);
        }

        public void Dispose()
        {
            _ws.Dispose();
        }
    }
}
